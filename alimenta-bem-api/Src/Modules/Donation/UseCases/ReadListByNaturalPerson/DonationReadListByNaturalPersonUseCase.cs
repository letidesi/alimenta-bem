using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.Repository;
using AlimentaBem.Src.Modules.Donation.UseCases.ReadListByNaturalPerson.DTO;

namespace AlimentaBem.Src.Modules.Donation.UseCases.ReadListByNaturalPerson;

public class DonationReadListByNaturalPersonUseCase
{
    private readonly IDonationData _donationData;

    public DonationReadListByNaturalPersonUseCase(AlimentaBemContext context)
    {
        _donationData = new DonationData(context);
    }

    public async Task<DonationReadListByNaturalPersonResponse> exec(Guid naturalPersonId)
    {
        var donations = await _donationData.ReadByNaturalPerson(naturalPersonId);

        return new DonationReadListByNaturalPersonResponse
        {
            donations = donations.Select(d => new DonationByNaturalPersonItem
            {
                id = d.id,
                organizationId = d.organizationId,
                organizationName = d.organization?.name ?? "Instituição",
                itemName = d.itemName,
                amountDonated = d.amountDonated,
                status = d.status,
                unavailableMessage = d.unavailableMessage,
                createdAt = d.createdAt,
                reviewedAt = d.reviewedAt,
                receivedAt = d.receivedAt,
            }).ToList(),
        };
    }
}
