using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.UpdateRole
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserUpdateRoleUseCase
    {
        private readonly AlimentaBemContext _context;
        private readonly Localizer _localizer;
        private readonly IUserData _userData;
        private readonly IUserOrganizationData _userOrganizationData;

        public UserUpdateRoleUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _context = context;
            _localizer = localizer;
            _userData = new UserData(context);
            _userOrganizationData = new UserOrganizationData(context);
        }

        public async Task<User> exec(Guid userId, string role, Guid? adminUserId = null)
        {
            if (!Enum.TryParse<EnumRole>(role, true, out var parsedRole))
                throw new Exception(_localizer["data:InvalidTypeParameter"]);

            var updatedUser = await _userData.UpdateRole(userId, parsedRole.ToString());

            if (updatedUser is null)
                throw new Exception(_localizer["user:UserNotFound"]);

            if (parsedRole == EnumRole.Admin && adminUserId.HasValue)
            {
                var adminOrgIds = await _userOrganizationData.GetOrganizationIdsByUser(adminUserId.Value);
                if (adminOrgIds.Count > 0)
                    await _userOrganizationData.LinkUserToOrganizations(userId, adminOrgIds);
            }

            return updatedUser;
        }
    }
}
