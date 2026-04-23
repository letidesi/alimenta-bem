using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository.Enums;
using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create
{
    using OrganizationRequirement = AlimentaBem.Src.Modules.OrganizationRequirement.Repository.OrganizationRequirement;

    public class OrganizationRequirementCreateMapper : Mapper<OrganizationRequirementCreateRequest, OrganizationRequirementCreateResponse, OrganizationRequirement>
    {

        public override OrganizationRequirement ToEntity(OrganizationRequirementCreateRequest req) => new()
        {
            organizationId = req.organizationId,
            itemName = req.itemName,
            quantity = req.quantity,
            type = EnumHelper.ToEnum<Priority>(req.type),
        };

        public override OrganizationRequirementCreateResponse FromEntity(OrganizationRequirement or) => new()
        {
            id = or.id,
            organizationId = or.organizationId,
            itemName = or.itemName,
            quantity = or.quantity,
            type = or.type.ToString(),
            createdAt = or.createdAt,
            updatedAt = or.updatedAt
        };
    }
}