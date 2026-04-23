using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.Repository.Enums;
using AlimentaBem.Src.Modules.Organization.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Create
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class OrganizationCreateMapper : Mapper<OrganizationCreateRequest, OrganizationCreateResponse, Organization>
    {

        public override Organization ToEntity(OrganizationCreateRequest req) => new()
        {
            name = req.name,
            type = EnumHelper.ToEnumOrNull<TypeOrganization>(req.type),
            description = req.description
        };

        public override OrganizationCreateResponse FromEntity(Organization o) => new()
        {
            id = o.id,
            name = o.name,
            type = o.type.ToString(),
            description = o.description,
            createdAt = o.createdAt,
            updatedAt = o.updatedAt
        };
    }
}