using AlimentaBem.Helpers;
using static AlimentaBem.Helpers.InputSanitizer;

namespace AlimentaBem.Src.Modules.User.UseCases.AdminCreate.DTO;

public class UserAdminCreateValidator : Validator<UserAdminCreateRequest>
{
    public UserAdminCreateValidator(Localizer localizer)
    {
        RuleFor(r => r.name)
            .NotEmpty()
            .WithMessage(localizer["data:NameRequired"])
            .MaximumLength(200)
            .WithMessage(localizer["data:NameRequired"])
            .Must(v => v == null || !ContainsHtml(v))
            .WithMessage("O campo nome não pode conter HTML ou scripts.");

        RuleFor(r => r.email)
            .NotEmpty()
            .WithMessage(localizer["data:EmailRequired"])
            .EmailAddress()
            .WithMessage(localizer["data:FormatOfEmailAddress"]);

        RuleFor(r => r.password)
            .NotEmpty()
            .WithMessage(localizer["user:PasswordRequired"])
            .MinimumLength(8)
            .WithMessage(localizer["user:PasswordShort"])
            .MaximumLength(128)
            .WithMessage(localizer["user:PasswordLong"]);

        RuleFor(r => r.role)
            .NotEmpty()
            .WithMessage(localizer["role:RoleRequired"]);
    }
}
