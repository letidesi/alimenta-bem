using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.Organization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.ReadList
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class OrganizationReadListUseCase
    {
        private Localizer _localizer;
        public IUserData _userData;
        public IOrganizationData _organizationData;

        public OrganizationReadListUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _organizationData = new OrganizationData(context);
        }

        public async Task<List<Organization>> exec()
        {

            var organizations = await _organizationData.ReadList();
            if (organizations.Count == 0)
                throw new Exception(_localizer["organization:UserNotFound"]);

            return organizations;
        }
    }
}