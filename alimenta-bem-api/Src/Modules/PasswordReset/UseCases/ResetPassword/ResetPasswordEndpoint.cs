using System.Security.Cryptography;
using System.Text;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.PasswordReset.UseCases.ResetPassword.DTO;
using AlimentaBem.Src.Modules.User.Repository;

namespace AlimentaBem.Src.Modules.PasswordReset.UseCases.ResetPassword;

public class ResetPasswordEndpoint : Endpoint<ResetPasswordRequest>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("user/reset-password");
        AllowAnonymous();
        Options(u => u.WithTags("user").RequireRateLimiting("reset-password"));
        Summary(s =>
        {
            s.Summary = "Reset password";
            s.Description = "Sets a new password using a valid reset token";
        });
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        try
        {
            // Hashear o token recebido para comparar com o hash armazenado
            var tokenHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(req.token)));

            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.token == tokenHash && !t.used && t.expiresAt > DateTimeOffset.UtcNow, ct);

            if (resetToken is null)
                ThrowError(_localizer["passwordReset:InvalidOrExpiredToken"]);

            var userData = new UserData(_context);
            var user = await userData.ReadOne(resetToken!.userId);

            if (user is null)
                ThrowError(_localizer["user:UserNotFound"]);

            user!.passwordHash = FormatPassword.GenerateHash(req.newPassword);
            resetToken.used = true;

            await _context.SaveChangesAsync(ct);

            await SendOkAsync(ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
