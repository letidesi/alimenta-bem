namespace AlimentaBem.Src.Modules.Organization.Repository;

public interface IOrganizationData
{
    Task<Organization> Create(Organization organization);
    Task<List<Organization>> ReadList();
    Task<List<Organization>> ReadListByIds(IEnumerable<Guid> ids);
    Task<Organization?> ReadOne(Guid id);
    Task<bool> CnpjExists(string cnpj, Guid? excludeId = null);
    Task<Organization> Update(Organization organization);
    Task SoftDelete(Organization organization);
}