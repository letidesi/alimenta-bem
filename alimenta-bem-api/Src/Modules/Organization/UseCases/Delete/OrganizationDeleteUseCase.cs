using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Delete;

public class OrganizationDeleteUseCase
{
    private readonly Localizer _localizer;
    private readonly IOrganizationData _organizationData;

    public OrganizationDeleteUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _localizer = localizer;
        _organizationData = new OrganizationData(context);
    }

    public async Task exec(Guid id)
    {
        var existing = await _organizationData.ReadOne(id);

        if (existing is null)
            throw new Exception(_localizer["organization:NotFound"]);

        await _organizationData.SoftDelete(existing);
    }
}
