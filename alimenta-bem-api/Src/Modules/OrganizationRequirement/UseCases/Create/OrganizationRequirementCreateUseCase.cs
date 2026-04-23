using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create
{
    using OrganizationRequirement = AlimentaBem.Src.Modules.OrganizationRequirement.Repository.OrganizationRequirement;

    public class OrganizationRequirementCreateUseCase
    {
        private Localizer _localizer;
        public IOrganizationRequirementData _organizationRequirementData;

        public OrganizationRequirementCreateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _organizationRequirementData = new OrganizationRequirementData(context);
        }

        public async Task<OrganizationRequirement> exec(OrganizationRequirement organizationRequirement)
        {
            var createOrganizationRequirement = await _organizationRequirementData.Create(organizationRequirement);

            if (createOrganizationRequirement is null)
                throw new Exception(_localizer["organizationRequirement:IndividualCreationFailed"]);

            return createOrganizationRequirement;
        }
    }
}