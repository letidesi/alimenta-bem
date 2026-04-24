using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Organization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Create
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class OrganizationCreateUseCase
    {
        private Localizer _localizer;
        public IOrganizationData _organizationData;

        public OrganizationCreateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _organizationData = new OrganizationData(context);
        }

        public async Task<Organization> exec(Organization naturalPerson)
        {

            var createOrganization = await _organizationData.Create(naturalPerson);
            
            if (createOrganization is null)
                throw new Exception(_localizer["organization:IndividualCreationFailed"]);

            return createOrganization;
        }
    }
}