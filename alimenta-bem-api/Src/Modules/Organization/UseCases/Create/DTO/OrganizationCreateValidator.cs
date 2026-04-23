using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Create.DTO;

public class Validator : Validator<OrganizationCreateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.name)
        .NotEmpty()
        .WithMessage(_localizer["data:NameRequired"]);
        
        RuleFor(request => request.type)
        .NotEmpty()
        .WithMessage(_localizer["data:TypeRequired"]);
    }
}