using AlimentaBem.Helpers;
using AlimentaBem.Context;

namespace AlimentaBem.Src.Modules.Organization.Repository;

public class OrganizationData : IOrganizationData
{
    private readonly AlimentaBemContext _context;

    public OrganizationData(AlimentaBemContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Organization> Create(Organization organization)
    {
        organization = (Organization)WhiteSpaces.RemoveFromAllStringProperty(organization);

        _context.Organizations.Add(organization);

        _ = await _context.SaveChangesAsync();

        return organization;
    }

    public Task<List<Organization>> ReadList()
    {
        return _context.Organizations.ToListAsync();
    }

    public Task<Organization?> ReadOne(Guid id)
    {
        return _context.Organizations
            .Where(o => o.id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Organization> Update(Organization organization)
    {
        organization = (Organization)WhiteSpaces.RemoveFromAllStringProperty(organization);

        _context.Organizations.Update(organization);

        _ = await _context.SaveChangesAsync();

        return organization;
    }

    public async Task SoftDelete(Organization organization)
    {
        organization.deletedAt = DateTimeOffset.UtcNow;
        _context.Organizations.Update(organization);
        _ = await _context.SaveChangesAsync();
    }
}