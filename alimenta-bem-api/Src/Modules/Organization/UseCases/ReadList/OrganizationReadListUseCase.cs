using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Organization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.ReadList
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class OrganizationReadListUseCase
    {
        public IOrganizationData _organizationData;

        public OrganizationReadListUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _organizationData = new OrganizationData(context);
        }

        public async Task<List<Organization>> exec()
        {
            return await _organizationData.ReadList();
        }
    }
}