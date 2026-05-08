using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.User.UseCases.AdminCreate.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.AdminCreate.DTO;

public class UserAdminCreateValidator : Validator<UserAdminCreateRequest>
{
    public UserAdminCreateValidator(Localizer localizer)
    {
        RuleFor(r => r.name)
            .NotEmpty()
            .WithMessage(localizer["data:NameRequired"]);

        RuleFor(r => r.email)
            .NotEmpty()
            .WithMessage(localizer["data:EmailRequired"])
            .EmailAddress()
            .WithMessage(localizer["data:FormatOfEmailAddress"]);

        RuleFor(r => r.password)
            .NotEmpty()
            .WithMessage(localizer["user:PasswordRequired"])
            .MinimumLength(12)
            .WithMessage(localizer["user:PasswordShort"])
            .MaximumLength(128)
            .WithMessage(localizer["user:PasswordLong"]);

        RuleFor(r => r.role)
            .NotEmpty()
            .WithMessage(localizer["role:RoleRequired"]);
    }
}
