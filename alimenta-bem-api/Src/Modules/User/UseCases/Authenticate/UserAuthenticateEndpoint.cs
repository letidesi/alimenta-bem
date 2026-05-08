using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Providers.Crypto;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;
using Microsoft.Extensions.DependencyInjection;

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
            Options(u => u.WithTags("user").RequireRateLimiting("auth"));
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

                var isProd = !HttpContext.RequestServices
                    .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

                HttpContext.Response.Cookies.Append("accessToken", tokens.accesstoken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = isProd,
                    SameSite = isProd ? SameSiteMode.None : SameSiteMode.Lax,
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                });

                HttpContext.Response.Cookies.Append("refreshToken", tokens.refreshtoken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = isProd,
                    SameSite = isProd ? SameSiteMode.None : SameSiteMode.Lax,
                    Path = "/",
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                });

                await SendAsync(new UserAuthenticateResponse
                {
                    userId = user.id,
                    role = user.roles.Select(r => r.type).FirstOrDefault(),
                    expiresAt = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds(),
                });
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