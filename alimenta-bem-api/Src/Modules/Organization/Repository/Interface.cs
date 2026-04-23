namespace AlimentaBem.Src.Modules.Organization.Repository;

public interface IOrganizationData
{
    Task<Organization> Create(Organization organization);
    Task<List<Organization>> ReadList();
    Task<Organization?> ReadOne(Guid id);
    Task<Organization> Update(Organization organization);
    Task SoftDelete(Organization organization);
}