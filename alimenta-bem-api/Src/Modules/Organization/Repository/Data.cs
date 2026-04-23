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
        return _context.Organizations
            .Select(o => new Organization
            {
                id = o.id,
                name = o.name,
            })
            .ToListAsync();
    }
}