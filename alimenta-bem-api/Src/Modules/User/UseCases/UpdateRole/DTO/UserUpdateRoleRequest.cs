namespace AlimentaBem.Src.Modules.User.UseCases.UpdateRole.DTO;

public class UserUpdateRoleRequest
{
    public Guid userId { get; set; }
    public string role { get; set; }
}
