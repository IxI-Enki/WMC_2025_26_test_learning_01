using FluentValidation;

namespace Application.Features.[ENTITY_PLURAL].Commands.[CommandName];

/// <summary>
/// Validator für [CommandName][ENTITY_NAME]Command.
/// </summary>
public sealed class [CommandName][ENTITY_NAME]CommandValidator 
    : AbstractValidator<[CommandName][ENTITY_NAME]Command>
{
    public [CommandName][ENTITY_NAME]CommandValidator()
    {
        // STRING VALIDATIONS
        RuleFor(x => x.[StringProperty])
            .NotEmpty().WithMessage("[StringProperty] ist erforderlich.")
            .MinimumLength([MIN]).WithMessage("[StringProperty] muss mindestens [MIN] Zeichen lang sein.")
            .MaximumLength([MAX]).WithMessage("[StringProperty] darf maximal [MAX] Zeichen lang sein.");
        
        // INT VALIDATIONS
        RuleFor(x => x.[IntProperty])
            .GreaterThan(0).WithMessage("[IntProperty] muss größer als 0 sein.")
            .LessThanOrEqualTo([MAX]).WithMessage("[IntProperty] darf maximal [MAX] sein.");
        
        // DATETIME VALIDATIONS
        RuleFor(x => x.[DateProperty])
            .GreaterThanOrEqualTo(DateTime.Now).WithMessage("[DateProperty] darf nicht in der Vergangenheit liegen.");
        
        // CUSTOM VALIDATIONS
        RuleFor(x => x.[Property])
            .Must([CustomCheck]).WithMessage("[Error message]");
    }
    
    private bool [CustomCheck]([TYPE] value)
    {
        // Custom validation logic
        return true;
    }
}

