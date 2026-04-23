using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Update;

public class OrganizationRequirementUpdateUseCase
{
    private readonly Localizer _localizer;
    private readonly IOrganizationRequirementData _organizationRequirementData;

    public OrganizationRequirementUpdateUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _localizer = localizer;
        _organizationRequirementData = new OrganizationRequirementData(context);
    }

    public async Task<Repository.OrganizationRequirement> exec(Repository.OrganizationRequirement organizationRequirement)
    {
        var existingRequirement = await _organizationRequirementData.ReadOne(organizationRequirement.id);

        if (existingRequirement is null)
            throw new Exception(_localizer["organizationRequirement:NotFound"]);

        existingRequirement.organizationId = organizationRequirement.organizationId;
        existingRequirement.itemName = organizationRequirement.itemName;
        existingRequirement.quantity = organizationRequirement.quantity;
        existingRequirement.type = organizationRequirement.type;

        return await _organizationRequirementData.Update(existingRequirement);
    }
}
