namespace AlimentaBem.Src.Modules.Donation.Repository;

public interface IDonationData
{
    Task<Donation> Create(Donation donation);
    Task<Donation> Update(Donation donation);
    Task<Donation?> ReadOneById(Guid donationId);
    Task<List<Donation>> ReadByOrganization(Guid organizationId);
    Task<List<Donation>> ReadByNaturalPerson(Guid naturalPersonId);
}