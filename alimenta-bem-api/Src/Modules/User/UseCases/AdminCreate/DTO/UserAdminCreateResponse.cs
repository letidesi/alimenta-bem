namespace AlimentaBem.Src.Modules.User.UseCases.AdminCreate.DTO;

public class UserAdminCreateResponse
{
    public Guid userId { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string role { get; set; }
}
