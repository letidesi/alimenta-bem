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
        Options(u => u.WithTags("user"));
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
            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t => t.token == req.token && !t.used && t.expiresAt > DateTime.UtcNow, ct);

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
