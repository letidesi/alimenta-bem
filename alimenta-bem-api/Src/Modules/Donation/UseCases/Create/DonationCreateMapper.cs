using AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.Donation.UseCases.Create
{
    using Donation = AlimentaBem.Src.Modules.Donation.Repository.Donation;

    public class DonationCreateMapper : Mapper<DonationCreateRequest, DonationCreateResponse, Donation>
    {

        public override Donation ToEntity(DonationCreateRequest req) => new()
        {
            naturalPersonId = req.naturalPersonId,
            organizationId = req.organizationId,
            itemName = req.itemName,
            amountDonated = req.amountDonated
        };

        public override DonationCreateResponse FromEntity(Donation d) => new()
        {
            id = d.id,
            naturalPersonId = d.naturalPersonId,
            organizationId = d.organizationId,
            itemName = d.itemName,
            amountDonated = d.amountDonated,
            createdAt = d.createdAt,
            updatedAt = d.updatedAt
        };
    }
}