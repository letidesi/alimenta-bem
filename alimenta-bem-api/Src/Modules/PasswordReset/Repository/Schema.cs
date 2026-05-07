namespace AlimentaBem.Src.Modules.PasswordReset.Repository;

public class PasswordResetToken
{
    public Guid id { get; set; } = Guid.NewGuid();
    public Guid userId { get; set; }
    public string token { get; set; }
    public DateTime expiresAt { get; set; }
    public bool used { get; set; } = false;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
}
