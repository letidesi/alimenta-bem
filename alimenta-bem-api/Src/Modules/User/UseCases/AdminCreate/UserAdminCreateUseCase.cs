using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.AdminCreate
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;
    using Role = AlimentaBem.Src.Modules.Role.Repository.Role;
    using UserOrganizationEntity = AlimentaBem.Src.Modules.UserOrganization.Repository.UserOrganization;

    public class UserAdminCreateUseCase
    {
        private readonly AlimentaBemContext _context;
        private readonly Localizer _localizer;
        private readonly IUserData _userData;
        private readonly IUserOrganizationData _userOrganizationData;
        private readonly isValidEmailFunction _isValidEmailFunction;

        public UserAdminCreateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _context = context;
            _localizer = localizer;
            _userData = new UserData(context);
            _userOrganizationData = new UserOrganizationData(context);
            _isValidEmailFunction = new isValidEmailFunction();
        }

        public async Task<User> exec(string name, string email, string password, string role, Guid adminUserId, List<Guid> organizationIds)
        {
            if (!_isValidEmailFunction.IsValidEmail(email))
                throw new Exception(_localizer["data:EmailInvalid"]);

            if (!Enum.TryParse<EnumRole>(role, true, out var parsedRole))
                throw new Exception(_localizer["data:InvalidTypeParameter"]);

            var normalizedEmail = email.ToLower().Trim();
            var normalizedName = name.Trim();

            var existing = await _userData.ReadOneByEmail(normalizedEmail);
            if (existing is not null)
                throw new Exception(_localizer["user:UserSameEmail"]);

            var adminOrgIds = await _userOrganizationData.GetOrganizationIdsByUser(adminUserId);

            foreach (var orgId in organizationIds)
                if (!adminOrgIds.Contains(orgId))
                    throw new Exception(_localizer["userOrganization:NoAccess"]);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new User
                {
                    name = normalizedName,
                    email = normalizedEmail,
                    passwordHash = FormatPassword.GenerateHash(password),
                    roles = new List<Role> { new Role { type = parsedRole.ToString() } }
                };

                user = await _userData.Create(user);

                if (organizationIds.Count > 0)
                    await _userOrganizationData.LinkUserToOrganizations(user.id, organizationIds);

                await transaction.CommitAsync();
                return user;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
