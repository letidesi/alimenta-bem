using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Organization.Repository;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Create
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;
    using UserOrganizationEntity = AlimentaBem.Src.Modules.UserOrganization.Repository.UserOrganization;

    public class OrganizationCreateUseCase
    {
        private readonly AlimentaBemContext _context;
        private Localizer _localizer;
        public IOrganizationData _organizationData;
        public IUserOrganizationData _userOrganizationData;

        public OrganizationCreateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _context = context;
            _localizer = localizer;
            _organizationData = new OrganizationData(context);
            _userOrganizationData = new UserOrganizationData(context);
        }

        public async Task<Organization> exec(Organization organization, Guid adminUserId)
        {
            if (!string.IsNullOrEmpty(organization.cnpj))
            {
                if (organization.cnpj.Length != 14)
                    throw new Exception(_localizer["organization:CnpjInvalid"]);

                if (await _organizationData.CnpjExists(organization.cnpj))
                    throw new Exception(_localizer["organization:CnpjAlreadyExists"]);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var created = await _organizationData.Create(organization);

                if (created is null)
                    throw new Exception(_localizer["organization:IndividualCreationFailed"]);

                await _userOrganizationData.Create(new UserOrganizationEntity
                {
                    userId = adminUserId,
                    organizationId = created.id
                });

                await transaction.CommitAsync();
                return created;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}