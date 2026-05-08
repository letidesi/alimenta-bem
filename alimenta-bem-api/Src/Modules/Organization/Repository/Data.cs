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
        return _context.Organizations.AsNoTracking().ToListAsync();
    }

    public Task<List<Organization>> ReadListByIds(IEnumerable<Guid> ids)
    {
        return _context.Organizations
            .Where(o => ids.Contains(o.id))
            .ToListAsync();
    }

    public Task<Organization?> ReadOne(Guid id)
    {
        return _context.Organizations
            .Where(o => o.id == id)
            .FirstOrDefaultAsync();
    }

    public Task<bool> CnpjExists(string cnpj, Guid? excludeId = null)
    {
        return _context.Organizations
            .Where(o => o.cnpj == cnpj && (excludeId == null || o.id != excludeId))
            .AnyAsync();
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