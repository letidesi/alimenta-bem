using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.User.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.ReadList
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserReadListUseCase
    {
        private readonly Localizer _localizer;
        private readonly IUserData _userData;

        public UserReadListUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _userData = new UserData(context);
        }

        public async Task<List<User>> exec()
        {
            return await _userData.ReadList();
        }
    }
}
