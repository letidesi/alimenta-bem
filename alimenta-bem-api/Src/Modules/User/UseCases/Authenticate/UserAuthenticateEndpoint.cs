using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Providers.Crypto;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.Authenticate
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserAuthenticateEndPoint : Endpoint<UserAuthenticateRequest, UserAuthenticateResponse>
    {
        public AlimentaBemContext _context { get; init; }
        public ICryptoProvider _crypto { get; init; }
        public Localizer _localizer { get; init; }
        private IUserData _userData;

        public override void Configure()
        {
            Post("user/authenticate");
            Options(u => u.WithTags("user"));
            Summary(s =>
            {
                s.Summary = "Create a new user";
                s.Description = "Register a user on the platform";
            });
            AllowAnonymous();
        }

        public override async Task HandleAsync(UserAuthenticateRequest req, CancellationToken ct)
        {
            try
            {
                var user = await GetUser(req);

                var useCase = new UserAuthenticateUseCase(_localizer, _crypto);

                var tokens = useCase.exec(req, user);

                await SendAsync(tokens);
            }
            catch (Exception e)
            {
                ThrowError(e.Message);
            }
        }
        private async Task<User> GetUser(UserAuthenticateRequest req)
        {
            _userData = new UserData(_context);
            var user = await _userData.ReadOneByEmail(req.email.Trim());

            if (user is null)
                throw new Exception(_localizer["user:LoginInvalid"]);

            return user;
        }
    }
}