using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.User.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.Create.DTO;

public class Validator : Validator<UserCreateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.name)
           .NotEmpty()
           .WithMessage(_localizer["data:NameRequired"]);

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