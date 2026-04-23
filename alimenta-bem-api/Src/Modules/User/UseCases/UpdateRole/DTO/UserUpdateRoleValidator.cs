using AlimentaBem.Helpers;

namespace AlimentaBem.Src.Modules.User.UseCases.UpdateRole.DTO;

public class Validator : Validator<UserUpdateRoleRequest>
{
    public Validator(Localizer localizer)
    {
        RuleFor(request => request.userId)
            .NotEmpty()
            .WithMessage(localizer["user:UserIdRequired"]);

        RuleFor(request => request.role)
            .NotEmpty()
            .WithMessage(localizer["data:TypeRequired"]);
    }
}
