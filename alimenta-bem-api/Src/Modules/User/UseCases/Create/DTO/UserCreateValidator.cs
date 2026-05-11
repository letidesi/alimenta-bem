using AlimentaBem.Helpers;
using static AlimentaBem.Helpers.InputSanitizer;

namespace AlimentaBem.Src.Modules.User.UseCases.Create.DTO;

public class Validator : Validator<UserCreateRequest>
{
    public Validator(Localizer localizer)
    {
        RuleFor(request => request.name)
           .NotEmpty()
           .WithMessage(localizer["data:NameRequired"])
           .MaximumLength(200)
           .WithMessage(localizer["data:NameRequired"])
           .Must(v => v == null || !ContainsHtml(v))
           .WithMessage("O campo nome não pode conter HTML ou scripts.");

        RuleFor(request => request.email)
            .NotEmpty()
            .WithMessage(localizer["data:EmailRequired"])
            .EmailAddress()
            .WithMessage(localizer["data:FormatOfEmailAddress"]);

        RuleFor(request => request.password)
            .NotEmpty()
            .WithMessage(localizer["user:PasswordRequired"])
            .MinimumLength(8)
            .WithMessage(localizer["user:PasswordShort"])
            .MaximumLength(128)
            .WithMessage(localizer["user:PasswordLong"]);
    }
}
