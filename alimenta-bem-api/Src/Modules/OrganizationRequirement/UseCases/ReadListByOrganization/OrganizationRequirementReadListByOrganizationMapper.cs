using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.ReadListByOrganization.DTO;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.ReadListByOrganization;

public class OrganizationRequirementReadListByOrganizationMapper : Mapper<OrganizationRequirementReadListByOrganizationRequest, OrganizationRequirementReadListByOrganizationResponse, List<Repository.OrganizationRequirement>>
{
    public override List<Repository.OrganizationRequirement> ToEntity(OrganizationRequirementReadListByOrganizationRequest req) =>
        new();

    public override OrganizationRequirementReadListByOrganizationResponse FromEntity(List<Repository.OrganizationRequirement> requirements) =>
        new()
        {
            requirements = requirements.Select(r => new OrganizationRequirementReadListByOrganizationResponse.OrganizationRequirementReadListByOrganizationItem
            {
                id = r.id,
                itemName = r.itemName,
                quantity = r.quantity,
                type = r.type.ToString()
            }).ToList()
        };
}
