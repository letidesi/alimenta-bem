using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;

public class Validator : Validator<UserAuthenticateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.email)
            .NotEmpty()
            .WithMessage(_localizer["data:EmailRequired"])
            .EmailAddress()
            .WithMessage(_localizer["data:FormatOfEmailAddress"]);

        RuleFor(request => request.password)
            .NotEmpty()
            .WithMessage(_localizer["user:PasswordRequired"])
            .MinimumLength(6)
            .WithMessage(_localizer["user:PasswordShort"])
            .MaximumLength(25)
            .WithMessage(_localizer["user:PasswordLong"]);
    }
}