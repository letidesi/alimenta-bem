using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.PasswordReset.Repository;
using AlimentaBem.Src.Modules.PasswordReset.UseCases.ForgotPassword.DTO;
using AlimentaBem.Src.Modules.User.Repository;
using Resend;

namespace AlimentaBem.Src.Modules.PasswordReset.UseCases.ForgotPassword;

public class ForgotPasswordEndpoint : Endpoint<ForgotPasswordRequest>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }
    public IResend _resend { get; init; }
    public IConfiguration _configuration { get; init; }
    public IWebHostEnvironment _env { get; init; }

    public override void Configure()
    {
        Post("user/forgot-password");
        AllowAnonymous();
        Options(u => u.WithTags("user"));
        Summary(s =>
        {
            s.Summary = "Request password reset";
            s.Description = "Sends a password reset link to the user's email";
        });
    }

    public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        try
        {
            var userData = new UserData(_context);
            var user = await userData.ReadOneByEmail(req.email.ToLower().Trim());

            // Sempre retorna 200 para não revelar se o email existe
            if (user is null)
            {
                await SendOkAsync(ct);
                return;
            }

            // Invalida tokens anteriores do usuário
            var oldTokens = await _context.PasswordResetTokens
                .Where(t => t.userId == user.id && !t.used && t.expiresAt > DateTime.UtcNow)
                .ToListAsync(ct);

            foreach (var old in oldTokens)
                old.used = true;

            var token = Guid.NewGuid().ToString("N");
            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                userId = user.id,
                token = token,
                expiresAt = DateTime.UtcNow.AddHours(1)
            });

            await _context.SaveChangesAsync(ct);

            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000";
            var resetLink = $"{frontendUrl}/reset-password?token={token}";

            if (_env.IsDevelopment())
            {
                Console.WriteLine($"[DEV] Reset link for {user.email}: {resetLink}");
            }
            else
            {
                var fromEmail = _configuration["Resend:FromEmail"] ?? "onboarding@resend.dev";
                await _resend.EmailSendAsync(new EmailMessage
                {
                    From = fromEmail,
                    To = user.email,
                    Subject = "Recuperação de senha — Alimenta Bem",
                    HtmlBody = $@"
                        <p>Olá, <strong>{user.name}</strong>!</p>
                        <p>Recebemos uma solicitação para redefinir sua senha.</p>
                        <p>Clique no botão abaixo para criar uma nova senha. O link expira em <strong>1 hora</strong>.</p>
                        <p><a href=""{resetLink}"" style=""background:#1677ff;color:#fff;padding:10px 20px;border-radius:6px;text-decoration:none"">Redefinir senha</a></p>
                        <p>Se você não solicitou isso, ignore este e-mail.</p>
                    "
                });
            }

            await SendOkAsync(ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
