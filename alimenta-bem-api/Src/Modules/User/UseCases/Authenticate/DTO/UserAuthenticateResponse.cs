namespace AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;

public class UserAuthenticateResponse
{
    public Guid userId { get; set; }
    public string? role { get; set; }
    public long expiresAt { get; set; }
}
