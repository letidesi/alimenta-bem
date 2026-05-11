using AlimentaBem.Helpers;
using static AlimentaBem.Helpers.InputSanitizer;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create.DTO;

public class Validator : Validator<OrganizationRequirementCreateRequest>
{
    public Validator(Localizer localizer)
    {
        RuleFor(request => request.organizationId)
        .NotEmpty()
        .WithMessage(localizer["organization:OrganizationIdRequired"]);

        RuleFor(request => request.itemName)
        .NotEmpty()
        .WithMessage(localizer["organizationRequirement:ItemNameRequired"])
        .MaximumLength(150)
        .WithMessage(localizer["organizationRequirement:ItemNameRequired"])
        .Must(v => v == null || !ContainsHtml(v))
        .WithMessage("O campo item não pode conter HTML ou scripts.");

        RuleFor(request => request.quantity)
        .NotEmpty()
        .WithMessage(localizer["organizationRequirement:QuantityRequired"])
        .GreaterThan(0)
        .WithMessage(localizer["organizationRequirement:QuantityRequired"])
        .LessThanOrEqualTo(100_000)
        .WithMessage("Quantidade inválida.");

        RuleFor(request => request.type)
        .NotEmpty()
        .WithMessage(localizer["data:TypeRequired"])
        .Must(v => v == null || new[] { "Alta", "Media", "Baixa" }.Contains(v))
        .WithMessage("Prioridade inválida.");
    }
}
