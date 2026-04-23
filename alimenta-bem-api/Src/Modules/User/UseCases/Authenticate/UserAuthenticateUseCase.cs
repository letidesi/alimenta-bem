using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Providers.Crypto;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.Authenticate
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserAuthenticateUseCase
    {
        private Localizer _localizer;
        private readonly ICryptoProvider _criptoProvider;
        public UserAuthenticateUseCase(Localizer localizer, ICryptoProvider cryptoProvider)
        {
            _localizer = localizer;
            _criptoProvider = cryptoProvider;
        }

        public UserAuthenticateResponse exec(UserAuthenticateRequest request, User user)
        {
            var passwordIsValid = FormatPassword.ComparePassword(request.password, user.passwordHash);
            if (!passwordIsValid)
                throw new Exception(_localizer["user:LoginInvalid"]);

            var tokens = new UserAuthenticateResponse()
            {
                accesstoken = _criptoProvider.generateAccessToken(user),
                refreshtoken = _criptoProvider.generateRefreshToken(user)
            };

            return tokens;
        }
    }
}