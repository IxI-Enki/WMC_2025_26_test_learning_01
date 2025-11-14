using Application.Common.Models;
using Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Pipeline;

/// <summary>
/// MediatR Pipeline Behavior für automatische Validierung mit FluentValidation.
/// Fängt auch Domain-Exceptions ab und wandelt sie in Result-Objekte um.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // FluentValidation
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));
                return CreateValidationErrorResult<TResponse>(errorMessage);
            }
        }

        // Domain-Exceptions abfangen
        try
        {
            return await next();
        }
        catch (DomainValidationException ex)
        {
            return CreateConflictResult<TResponse>($"{ex.Property}: {ex.Message}");
        }
    }

    private static TResponse CreateValidationErrorResult<T>(string message)
    {
        var resultType = typeof(T);
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = resultType.GetGenericArguments()[0];
            var method = typeof(Result<>).MakeGenericType(valueType)
                .GetMethod(nameof(Result<object>.ValidationError));
            return (TResponse)method!.Invoke(null, [message])!;
        }
        throw new InvalidOperationException("TResponse must be Result<T>");
    }

    private static TResponse CreateConflictResult<T>(string message)
    {
        var resultType = typeof(T);
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = resultType.GetGenericArguments()[0];
            var method = typeof(Result<>).MakeGenericType(valueType)
                .GetMethod(nameof(Result<object>.Conflict));
            return (TResponse)method!.Invoke(null, [message])!;
        }
        if (resultType == typeof(Result))
        {
            var method = typeof(Result).GetMethod(nameof(Result.Conflict));
            return (TResponse)method!.Invoke(null, [message])!;
        }
        throw new InvalidOperationException("TResponse must be Result<T> or Result");
    }
}
