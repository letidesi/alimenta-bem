using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Delete;

public class OrganizationRequirementDeleteUseCase
{
    private readonly AlimentaBemContext _context;
    private readonly Localizer _localizer;
    private readonly IOrganizationRequirementData _organizationRequirementData;

    public OrganizationRequirementDeleteUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _context = context;
        _localizer = localizer;
        _organizationRequirementData = new OrganizationRequirementData(context);
    }

    public async Task exec(Guid id, Guid adminUserId)
    {
        var existingRequirement = await _organizationRequirementData.ReadOne(id);

        if (existingRequirement is null)
            throw new Exception(_localizer["organizationRequirement:NotFound"]);

        await AdminOrganizationGuard.EnsureAccess(_context, adminUserId, existingRequirement.organizationId, _localizer);

        await _organizationRequirementData.Delete(existingRequirement);
    }
}
