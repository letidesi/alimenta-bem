using AlimentaBem.Helpers;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert.DTO;

public class Validator : Validator<NaturalPersonAdminUpsertRequest>
{
    public Validator(Localizer localizer)
    {
        RuleFor(request => request.name)
            .NotEmpty()
            .WithMessage(localizer["data:NameRequired"]);

        RuleFor(request => request.email)
            .NotEmpty()
            .WithMessage(localizer["data:EmailRequired"])
            .EmailAddress()
            .WithMessage(localizer["data:FormatOfEmailAddress"]);

        RuleFor(request => request.password)
            .NotEmpty()
            .WithMessage(localizer["user:PasswordRequired"])
            .MinimumLength(6)
            .WithMessage(localizer["user:PasswordShort"])
            .MaximumLength(25)
            .WithMessage(localizer["user:PasswordLong"]);

        RuleFor(request => request.age)
            .NotEmpty()
            .WithMessage(localizer["naturalPerson:AgeRequired"]);

        RuleFor(request => request.birthdayDate)
            .NotEmpty()
            .WithMessage(localizer["naturalPerson:BirthdayDateRequired"]);
    }
}
