namespace AlimentaBem.Src.Modules.User.UseCases.Logout;

public class UserLogoutEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("user/logout");
        Options(u => u.WithTags("user"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var expired = new CookieOptions
        {
            HttpOnly = true,
            Secure = !HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
            SameSite = HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>().IsDevelopment()
                ? SameSiteMode.Lax
                : SameSiteMode.None,
            Path = "/",
            Expires = DateTimeOffset.UnixEpoch,
        };

        HttpContext.Response.Cookies.Append("accessToken", "", expired);
        HttpContext.Response.Cookies.Append("refreshToken", "", expired);

        await SendOkAsync(ct);
    }
}
