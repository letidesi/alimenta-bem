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

        if (!string.IsNullOrEmpty(organization.cnpj))
        {
            if (organization.cnpj.Length != 14)
                throw new Exception(_localizer["organization:CnpjInvalid"]);

            if (await _organizationData.CnpjExists(organization.cnpj, excludeId: organization.id))
                throw new Exception(_localizer["organization:CnpjAlreadyExists"]);
        }

        existing.name = organization.name;
        existing.type = organization.type;
        existing.description = organization.description;
        existing.cnpj = organization.cnpj;

        return await _organizationData.Update(existing);
    }
}
