namespace AlimentaBem.Src.Modules.Organization.Repository;

public interface IOrganizationData
{
    Task<Organization> Create(Organization organization);
    Task<List<Organization>> ReadList();
}