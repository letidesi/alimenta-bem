using AlimentaBem.Helpers;
using AlimentaBem.Context;

namespace AlimentaBem.Src.Modules.Donation.Repository;

public class DonationData : IDonationData
{
    private readonly AlimentaBemContext _context;

    public DonationData(AlimentaBemContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Donation> Create(Donation naturalPerson)
    {
        naturalPerson = (Donation)WhiteSpaces.RemoveFromAllStringProperty(naturalPerson);

        _context.Donations.Add(naturalPerson);

        _ = await _context.SaveChangesAsync();

        return naturalPerson;
    }
}