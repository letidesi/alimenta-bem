using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Update;

public class OrganizationUpdateUseCase
{
    private readonly Localizer _localizer;
    private readonly IOrganizationData _organizationData;

    public OrganizationUpdateUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _localizer = localizer;
        _organizationData = new OrganizationData(context);
    }

    public async Task<Repository.Organization> exec(Repository.Organization organization)
    {
        var existing = await _organizationData.ReadOne(organization.id);

        if (existing is null)
            throw new Exception(_localizer["organization:NotFound"]);

        existing.name = organization.name;
        existing.type = organization.type;
        existing.description = organization.description;

        return await _organizationData.Update(existing);
    }
}
