namespace AlimentaBem.Src.Modules.Donation.Repository;

public interface IDonationData
{
    Task<Donation> Create(Donation donation);
}