using AlimentaBem.Helpers;
using static AlimentaBem.Helpers.InputSanitizer;

namespace AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;

public class Validator : Validator<DonationCreateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.naturalPersonId)
            .NotEmpty()
            .WithMessage(_localizer["naturalPerson:NaturalPersonIdRequired"]);

        RuleFor(request => request.organizationId)
            .NotEmpty()
            .WithMessage(_localizer["organization:OrganizationIdRequired"]);

        RuleFor(request => request.itemName)
            .NotEmpty()
            .WithMessage(_localizer["donation:ItemNameRequired"])
            .MaximumLength(150)
            .WithMessage(_localizer["donation:ItemNameRequired"])
            .Must(v => v == null || !ContainsHtml(v))
            .WithMessage("O campo item não pode conter HTML ou scripts.");

        RuleFor(request => request.amountDonated)
            .NotEmpty()
            .WithMessage(_localizer["donation:AmountDonatedRequired"])
            .GreaterThan(0)
            .WithMessage(_localizer["donation:AmountDonatedRequired"])
            .LessThanOrEqualTo(100_000)
            .WithMessage("Quantidade doada inválida.");
    }
}