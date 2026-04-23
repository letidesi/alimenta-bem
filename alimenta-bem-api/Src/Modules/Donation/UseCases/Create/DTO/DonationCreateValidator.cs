using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;

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
            .WithMessage(_localizer["donation:ItemNameRequired"]);

        RuleFor(request => request.amountDonated)
            .NotEmpty()
            .WithMessage(_localizer["donation:AmountDonatedRequired"]);
    }
}