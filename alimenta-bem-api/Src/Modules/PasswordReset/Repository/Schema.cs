namespace AlimentaBem.Src.Modules.PasswordReset.Repository;

public class PasswordResetToken
{
    public Guid id { get; set; } = Guid.NewGuid();
    public Guid userId { get; set; }
    public string token { get; set; }
    public DateTimeOffset expiresAt { get; set; }
    public bool used { get; set; } = false;
    public DateTimeOffset createdAt { get; set; } = DateTimeOffset.UtcNow;
}
