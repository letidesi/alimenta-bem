using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.Repository;
using AlimentaBem.Src.Modules.Donation.UseCases.ReadListByOrganization.DTO;

namespace AlimentaBem.Src.Modules.Donation.UseCases.ReadListByOrganization;

public class DonationReadListByOrganizationUseCase
{
    private readonly IDonationData _donationData;

    public DonationReadListByOrganizationUseCase(AlimentaBemContext context)
    {
        _donationData = new DonationData(context);
    }

    public async Task<DonationReadListByOrganizationResponse> exec(Guid organizationId)
    {
        var donations = await _donationData.ReadByOrganization(organizationId);

        return new DonationReadListByOrganizationResponse
        {
            donations = donations.Select(d => new DonationByOrganizationItem
            {
                id = d.id,
                naturalPersonId = d.naturalPersonId,
                donorName = string.IsNullOrWhiteSpace(d.naturalPerson?.socialName)
                    ? d.naturalPerson?.name ?? "Doador"
                    : d.naturalPerson!.socialName!,
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
