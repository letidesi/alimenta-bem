using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.Repository.Enums;
using AlimentaBem.Src.Modules.Organization.UseCases.Update.DTO;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Update;

using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

public class OrganizationUpdateMapper : Mapper<OrganizationUpdateRequest, OrganizationUpdateResponse, Organization>
{
    public override Organization ToEntity(OrganizationUpdateRequest req) => new()
    {
        id = req.id,
        name = req.name,
        type = EnumHelper.ToEnumOrNull<TypeOrganization>(req.type),
        description = req.description,
    };

    public override OrganizationUpdateResponse FromEntity(Organization o) => new()
    {
        id = o.id,
        name = o.name,
        type = o.type?.ToString(),
        description = o.description,
        createdAt = o.createdAt,
        updatedAt = o.updatedAt,
    };
}
