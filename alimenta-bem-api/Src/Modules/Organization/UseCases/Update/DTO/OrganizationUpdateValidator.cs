using AlimentaBem.Helpers;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Update.DTO;

public class Validator : Validator<OrganizationUpdateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.id)
            .NotEmpty()
            .WithMessage(_localizer["organization:OrganizationIdRequired"]);

        RuleFor(request => request.name)
            .NotEmpty()
            .WithMessage(_localizer["data:NameRequired"]);

        RuleFor(request => request.type)
            .NotEmpty()
            .WithMessage(_localizer["data:TypeRequired"]);
    }
}
