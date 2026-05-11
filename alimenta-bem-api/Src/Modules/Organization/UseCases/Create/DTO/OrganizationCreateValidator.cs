using AlimentaBem.Helpers;
using static AlimentaBem.Helpers.InputSanitizer;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Create.DTO;

public class Validator : Validator<OrganizationCreateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.name)
        .NotEmpty()
        .WithMessage(_localizer["data:NameRequired"])
        .MaximumLength(150)
        .WithMessage(_localizer["data:NameRequired"])
        .Must(v => v == null || !ContainsHtml(v))
        .WithMessage("O campo nome não pode conter HTML ou scripts.");

        RuleFor(request => request.description)
        .MaximumLength(500)
        .WithMessage("Descrição deve ter no máximo 500 caracteres.")
        .Must(v => v == null || !ContainsHtml(v))
        .WithMessage("O campo descrição não pode conter HTML ou scripts.")
        .When(r => r.description != null);

        RuleFor(request => request.type)
        .NotEmpty()
        .WithMessage(_localizer["data:TypeRequired"])
        .Must(v => v == null || new[] { "ONG", "Escola", "Igreja", "Outros" }.Contains(v))
        .WithMessage("Tipo de instituição inválido.");
    }
}