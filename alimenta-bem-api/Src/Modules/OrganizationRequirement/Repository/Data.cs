using AlimentaBem.Helpers;
using AlimentaBem.Context;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.Repository;

public class OrganizationRequirementData : IOrganizationRequirementData
{
    private readonly AlimentaBemContext _context;

    public OrganizationRequirementData(AlimentaBemContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<OrganizationRequirement> Create(OrganizationRequirement organizationRequirement)
    {
        organizationRequirement = (OrganizationRequirement)WhiteSpaces.RemoveFromAllStringProperty(organizationRequirement);

        _context.OrganizationRequirements.Add(organizationRequirement);

        _ = await _context.SaveChangesAsync();

        return organizationRequirement;
    }

    public async Task<List<OrganizationRequirement>> ReadListByOrganizationId(Guid organizationId)
    {
        var requirements = await _context.OrganizationRequirements
            .Where(r => r.organizationId == organizationId)
            .OrderByDescending(r => r.updatedAt)
            .ToListAsync();

        return requirements;
    }

    public async Task<OrganizationRequirement?> ReadOne(Guid id)
    {
        return await _context.OrganizationRequirements.Where(r => r.id == id).FirstOrDefaultAsync();
    }

    public async Task<OrganizationRequirement> Update(OrganizationRequirement organizationRequirement)
    {
        organizationRequirement = (OrganizationRequirement)WhiteSpaces.RemoveFromAllStringProperty(organizationRequirement);

        _context.OrganizationRequirements.Update(organizationRequirement);

        _ = await _context.SaveChangesAsync();

        return organizationRequirement;
    }

    public async Task Delete(OrganizationRequirement organizationRequirement)
    {
        _context.OrganizationRequirements.Remove(organizationRequirement);

        _ = await _context.SaveChangesAsync();
    }
}