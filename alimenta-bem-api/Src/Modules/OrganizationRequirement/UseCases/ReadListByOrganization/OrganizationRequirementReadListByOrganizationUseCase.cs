using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.ReadListByOrganization;

public class OrganizationRequirementReadListByOrganizationUseCase
{
    private readonly Localizer _localizer;
    private readonly IOrganizationRequirementData _organizationRequirementData;

    public OrganizationRequirementReadListByOrganizationUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _localizer = localizer;
        _organizationRequirementData = new OrganizationRequirementData(context);
    }

    public async Task<List<Repository.OrganizationRequirement>> exec(Guid organizationId)
    {
        if (organizationId == Guid.Empty)
            throw new Exception(_localizer["organizationRequirement:NotFound"]);

        var requirements = await _organizationRequirementData.ReadListByOrganizationId(organizationId);
        return requirements;
    }
}
