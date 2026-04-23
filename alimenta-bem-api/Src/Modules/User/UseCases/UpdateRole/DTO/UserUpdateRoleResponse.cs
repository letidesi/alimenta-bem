namespace AlimentaBem.Src.Modules.User.UseCases.UpdateRole.DTO;

public class UserUpdateRoleResponse
{
    public Guid userId { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string role { get; set; }
}
