using Application.Common.Exceptions;
using Application.Common.Models;
using Domain.Exceptions;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application.Pipeline;

/// <summary>
/// Führt alle FluentValidation-Validatoren für eine Anfrage aus, bevor der Handler aufgerufen wird.
/// Ziel: Einheitliches Fehlerverhalten über die MediatR-Pipeline.
/// Fängt ALLE Exceptions ab und wandelt sie in Result-Objekte um.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // FluentValidation: Alle registrierten Validatoren ausführen
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f is not null).ToList();
            if (failures.Count != 0)
            {
                var message = string.Join(" | ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
                return CreateFailureResponse(nameof(Result.ValidationError), message);
            }
        }

        // Alle weiteren Exceptions abfangen und in Result-Typ umwandeln
        try
        {
            return await next();
        }
        catch (ValidationException ex)
        {
            var message = string.Join(" | ", ex.Errors.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            return CreateFailureResponse(nameof(Result.ValidationError), message);
        }
        catch (DomainValidationException ex)
        {
            var message = $"{ex.Property}: {ex.Message}";
            return CreateFailureResponse(nameof(Result.ValidationError), message);
        }
        catch (NotFoundException ex)
        {
            return CreateFailureResponse(nameof(Result.NotFound), ex.Message);
        }
        catch (ConcurrencyException)
        {
            return CreateFailureResponse(nameof(Result.Conflict), "Die Daten wurden zwischenzeitlich geändert. Bitte erneut versuchen.");
        }
        catch (Exception ex)
        {
            return CreateFailureResponse(nameof(Result.Error), ex.Message);
        }
    }

    private static TResponse CreateFailureResponse(string factoryMethodName, string? message)
    {
        var responseType = typeof(TResponse);

        // Non-generic Result
        if (responseType == typeof(Result))
        {
            return factoryMethodName switch
            {
                nameof(Result.ValidationError) => (TResponse)(object)Result.ValidationError(message ?? string.Empty),
                nameof(Result.NotFound) => (TResponse)(object)Result.NotFound(message ?? string.Empty),
                nameof(Result.Conflict) => (TResponse)(object)Result.Conflict(message ?? string.Empty),
                nameof(Result.Error) => (TResponse)(object)Result.Error(message ?? string.Empty),
                _ => (TResponse)(object)Result.Error(message ?? string.Empty)
            };
        }

        // Generic Result<T>
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericType = responseType.GetGenericArguments()[0];

            return factoryMethodName switch
            {
                nameof(Result.ValidationError) => CreateGenericResult<TResponse>(genericType, (g) =>
                    typeof(Result<>).MakeGenericType(g)
                        .GetMethod(nameof(Result<object>.ValidationError), BindingFlags.Public | BindingFlags.Static)!
                        .Invoke(null, [message ?? string.Empty])),
                nameof(Result.NotFound) => CreateGenericResult<TResponse>(genericType, (g) =>
                    typeof(Result<>).MakeGenericType(g)
                        .GetMethod(nameof(Result<object>.NotFound), BindingFlags.Public | BindingFlags.Static)!
                        .Invoke(null, [message ?? string.Empty])),
                nameof(Result.Conflict) => CreateGenericResult<TResponse>(genericType, (g) =>
                    typeof(Result<>).MakeGenericType(g)
                        .GetMethod(nameof(Result<object>.Conflict), BindingFlags.Public | BindingFlags.Static)!
                        .Invoke(null, [message ?? string.Empty])),
                nameof(Result.Error) => CreateGenericResult<TResponse>(genericType, (g) =>
                    typeof(Result<>).MakeGenericType(g)
                        .GetMethod(nameof(Result<object>.Error), BindingFlags.Public | BindingFlags.Static)!
                        .Invoke(null, [message ?? string.Empty])),
                _ => CreateGenericResult<TResponse>(genericType, (g) =>
                    typeof(Result<>).MakeGenericType(g)
                        .GetMethod(nameof(Result<object>.Error), BindingFlags.Public | BindingFlags.Static)!
                        .Invoke(null, [message ?? string.Empty]))
            };
        }

        // Fallback
        return (TResponse)(object)Result.Error(message ?? string.Empty);
    }

    private static TResult CreateGenericResult<TResult>(Type genericType, Func<Type, object?> factory)
    {
        var result = factory(genericType);
        return (TResult)result!;
    }
}
