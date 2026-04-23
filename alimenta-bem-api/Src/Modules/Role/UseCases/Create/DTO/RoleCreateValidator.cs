using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.Role.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.Role.UseCases.Create.DTO;

public class Validator : Validator<RoleCreateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.userId)
            .NotEmpty()
            .WithMessage(_localizer["user:UserIdRequired"]);

        RuleFor(request => request.type)
            .IsEnumName(typeof(EnumRole))
            .WithMessage(_localizer["data:InvalidTypeParameter"]);
    }
}