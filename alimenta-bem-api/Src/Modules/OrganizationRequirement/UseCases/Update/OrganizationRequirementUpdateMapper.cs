using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository.Enums;
using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Update.DTO;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Update;

public class OrganizationRequirementUpdateMapper : Mapper<OrganizationRequirementUpdateRequest, OrganizationRequirementUpdateResponse, Repository.OrganizationRequirement>
{
    public override Repository.OrganizationRequirement ToEntity(OrganizationRequirementUpdateRequest req) => new()
    {
        id = req.id,
        organizationId = req.organizationId,
        itemName = req.itemName,
        quantity = req.quantity,
        type = EnumHelper.ToEnum<Priority>(req.type),
    };

    public override OrganizationRequirementUpdateResponse FromEntity(Repository.OrganizationRequirement requirement) => new()
    {
        id = requirement.id,
        organizationId = requirement.organizationId,
        itemName = requirement.itemName,
        quantity = requirement.quantity,
        type = requirement.type.ToString(),
        createdAt = requirement.createdAt,
        updatedAt = requirement.updatedAt,
    };
}
