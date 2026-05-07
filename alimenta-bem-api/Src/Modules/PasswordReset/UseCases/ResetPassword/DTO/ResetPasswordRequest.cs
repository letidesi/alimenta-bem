namespace AlimentaBem.Src.Modules.PasswordReset.UseCases.ResetPassword.DTO;

public class ResetPasswordRequest
{
    public string token { get; set; }
    public string newPassword { get; set; }
}
