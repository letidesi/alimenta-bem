using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.User.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.ReadOne
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserReadOneUseCase
    {
        private Localizer _localizer;
        public IUserData _userData;
        public isValidEmailFunction isValidEmailFunction { get; set; }
        public UserReadOneUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _userData = new UserData(context);
            _localizer = localizer;
            isValidEmailFunction = new isValidEmailFunction();
        }

        public async Task<User> exec(Guid userId)
        {
            // var emailValidation = isValidEmailFunction.IsValidEmail(user.email);
            // if (emailValidation is false) throw new Exception(_localizer["data:EmailInvalid"]);

            var targetUser = await _userData.ReadOne(userId);
            if (targetUser is null)
                throw new Exception(_localizer["user:UserNotFound"]);

            return targetUser;
        }
    }
}