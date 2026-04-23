namespace AlimentaBem.Src.Modules.OrganizationRequirement.Repository;

public interface IOrganizationRequirementData
{
    Task<OrganizationRequirement> Create(OrganizationRequirement organizationRequirement);
    Task<List<OrganizationRequirement>> ReadListByOrganizationId(Guid organizationId);
    Task<OrganizationRequirement?> ReadOne(Guid id);
    Task<OrganizationRequirement> Update(OrganizationRequirement organizationRequirement);
    Task Delete(OrganizationRequirement organizationRequirement);
}