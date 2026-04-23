using AlimentaBem.Helpers;
using AlimentaBem.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Donation> Update(Donation donation)
    {
        donation = (Donation)WhiteSpaces.RemoveFromAllStringProperty(donation);

        _context.Donations.Update(donation);

        _ = await _context.SaveChangesAsync();

        return donation;
    }

    public async Task<Donation?> ReadOneById(Guid donationId)
    {
        return await _context.Donations
            .Include(d => d.organization)
            .Include(d => d.naturalPerson)
            .FirstOrDefaultAsync(d => d.id == donationId);
    }

    public async Task<List<Donation>> ReadByOrganization(Guid organizationId)
    {
        return await _context.Donations
            .Include(d => d.naturalPerson)
            .Where(d => d.organizationId == organizationId)
            .OrderByDescending(d => d.createdAt)
            .ToListAsync();
    }

    public async Task<List<Donation>> ReadByNaturalPerson(Guid naturalPersonId)
    {
        return await _context.Donations
            .Include(d => d.organization)
            .Where(d => d.naturalPersonId == naturalPersonId)
            .OrderByDescending(d => d.createdAt)
            .ToListAsync();
    }
}