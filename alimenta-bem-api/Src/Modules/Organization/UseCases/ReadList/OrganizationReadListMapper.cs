using AlimentaBem.Src.Modules.Organization.UseCases.ReadList.DTO;

namespace AlimentaBem.Src.Modules.Organization.UseCases.ReadList
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class OrganizationReadListMapper : ResponseMapper<OrganizationReadListResponse, List<Organization>>
    {
        public override OrganizationReadListResponse FromEntity(List<Organization> list) => new()
        {
            organizations = list.Select(o => new OrganizationReadListResponse.OrganizationReadListItem()
            {
                id = o.id,
                name = o.name
            }).ToList()
        };
    }
}