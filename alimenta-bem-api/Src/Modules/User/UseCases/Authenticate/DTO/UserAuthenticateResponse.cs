namespace AlimentaBem.Src.Modules.User.UseCases.Authenticate.DTO;

public class UserAuthenticateResponse
{
    public string accesstoken { get; set; }
    public string refreshtoken { get; set; }
}