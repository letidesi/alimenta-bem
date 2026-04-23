using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.UpdateRole
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserUpdateRoleUseCase
    {
        private readonly Localizer _localizer;
        private readonly IUserData _userData;

        public UserUpdateRoleUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _userData = new UserData(context);
        }

        public async Task<User> exec(Guid userId, string role)
        {
            if (!Enum.TryParse<EnumRole>(role, true, out var parsedRole))
                throw new Exception(_localizer["data:InvalidTypeParameter"]);

            var updatedUser = await _userData.UpdateRole(userId, parsedRole.ToString());

            if (updatedUser is null)
                throw new Exception(_localizer["user:UserNotFound"]);

            return updatedUser;
        }
    }
}
