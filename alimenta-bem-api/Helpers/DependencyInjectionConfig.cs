using AlimentaBem.Helpers;
using AlimentaBem.Src.Providers.Crypto;

public static class DependencyInjectionConfig
{
    public static void Register_Services(IServiceCollection services)
    {
        services.AddSingleton<Localizer>();
        services.AddSingleton<ICryptoProvider, CryptoService>();
        services.AddSingleton<CryptoService>();

    }
}
