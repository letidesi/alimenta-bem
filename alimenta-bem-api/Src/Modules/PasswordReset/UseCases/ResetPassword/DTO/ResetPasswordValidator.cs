using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.PasswordReset.UseCases.ResetPassword.DTO;

namespace AlimentaBem.Src.Modules.PasswordReset.UseCases.ResetPassword.DTO;

public class ResetPasswordValidator : Validator<ResetPasswordRequest>
{
    public ResetPasswordValidator(Localizer localizer)
    {
        RuleFor(r => r.token)
            .NotEmpty()
            .WithMessage(localizer["passwordReset:TokenRequired"]);

        RuleFor(r => r.newPassword)
            .NotEmpty()
            .WithMessage(localizer["user:PasswordRequired"])
            .MinimumLength(8)
            .WithMessage(localizer["user:PasswordShort"])
            .MaximumLength(128)
            .WithMessage(localizer["user:PasswordLong"]);
    }
}
