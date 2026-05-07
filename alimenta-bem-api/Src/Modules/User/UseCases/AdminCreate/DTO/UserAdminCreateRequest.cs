namespace AlimentaBem.Src.Modules.User.UseCases.AdminCreate.DTO;

public class UserAdminCreateRequest
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string role { get; set; }
    public List<Guid> organizationIds { get; set; } = new();
}
