using AlimentaBem.Helpers;
using AlimentaBem.Src.Providers.Crypto;
using Resend;

public static class DependencyInjectionConfig
{
    public static void Register_Services(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Localizer>();
        services.AddSingleton<ICryptoProvider, CryptoService>();
        services.AddSingleton<CryptoService>();

        services.AddHttpClient<ResendClient>();
        services.Configure<ResendClientOptions>(o =>
        {
            o.ApiToken = configuration["Resend:ApiKey"]
                ?? Environment.GetEnvironmentVariable("RESEND_API_KEY")!;
        });
        services.AddTransient<IResend, ResendClient>();
    }
}
