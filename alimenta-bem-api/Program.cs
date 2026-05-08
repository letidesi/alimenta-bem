global using FastEndpoints;
global using FluentValidation;
global using Microsoft.EntityFrameworkCore;
using AlimentaBem.Helpers;
using AlimentaBem.Context;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Threading.RateLimiting;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var isStaging = builder.Environment.IsStaging();
        var isDevelopment = builder.Environment.IsDevelopment();


        ConfigureServices(builder, isDevelopment);

        var app = builder.Build();

        ConfigureApp(app, isStaging, isDevelopment);

        app.Run();
    }

    public static void ConfigureServices(WebApplicationBuilder builder, bool isDevelopment)
    {
        dotenv.net.DotEnv.Load();

        string connectionString =
            builder.Configuration.GetConnectionString("sqlConnection") is { Length: > 0 } cs
                ? cs
                : Environment.GetEnvironmentVariable("DATABASE_URL")!;

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddDbContext<AlimentaBemContext>(options =>
            options.UseNpgsql(connectionString));

        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

        builder.Services.AddHttpContextAccessor();

        var allowedOrigins = new List<string> { "http://localhost:3000" };
        var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
        if (!string.IsNullOrEmpty(frontendUrl))
            allowedOrigins.Add(frontendUrl);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigins",
                b => b
                  .WithOrigins(allowedOrigins.ToArray())
                  .WithMethods("GET", "POST", "PUT", "DELETE")
                  .WithHeaders("Authorization", "Content-Type", "Accept-Language")
                  .AllowCredentials()
            );
        });


        var publicKeyContent = Environment.GetEnvironmentVariable("RSA_PUBLIC_KEY");
        string publicKeyText;

        if (!string.IsNullOrEmpty(publicKeyContent))
        {
            publicKeyText = publicKeyContent.Replace("\\n", "\n");
        }
        else
        {
            var publicKeyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public.key");
            publicKeyText = File.ReadAllText(publicKeyPath);
        }

        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyText.ToCharArray());

        var rsaSecurityKey = new RsaSecurityKey(rsa);

        // Configurar autenticação e autorização
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "Alimentabem",
                    ValidAudience = "ABem",

                    IssuerSigningKey = rsaSecurityKey,

                    RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        if (ctx.Request.Cookies.TryGetValue("accessToken", out var cookieToken))
                            ctx.Token = cookieToken;
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        });

        // Outros serviços
        DependencyInjectionConfig.Register_Services(builder.Services, builder.Configuration);
        CultureInfoConfig.Configure(builder.Services);

        builder.Services.AddFastEndpoints();

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // 5 tentativas de login por IP por minuto
            options.AddFixedWindowLimiter("auth", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromMinutes(1);
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 0;
            });

            // 3 pedidos de reset por IP por hora
            options.AddFixedWindowLimiter("forgot-password", o =>
            {
                o.PermitLimit = 3;
                o.Window = TimeSpan.FromHours(1);
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 0;
            });

            // 5 usos de token de reset por IP por hora
            options.AddFixedWindowLimiter("reset-password", o =>
            {
                o.PermitLimit = 5;
                o.Window = TimeSpan.FromHours(1);
                o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                o.QueueLimit = 0;
            });
        });

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocument(document =>
        {
            document.Title = "AlimentaBem";
            document.Version = "v1";
            document.OperationProcessors.Add(new PatchOperationProcessor());
            document.OperationProcessors.Add(new AcceptLanguageHeaderOperationProcessor(builder.Services.BuildServiceProvider().GetService<IOptions<RequestLocalizationOptions>>()));
        });
    }

    public static void ConfigureApp(WebApplication app, bool isStaging, bool isDevelopment)
    {
        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            await next();
        });

        app.UseCors("AllowOrigins");
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints(c => c.Serializer.Options.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        app.UseRequestLocalization();
        if (isStaging || isDevelopment)
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }


    }

}