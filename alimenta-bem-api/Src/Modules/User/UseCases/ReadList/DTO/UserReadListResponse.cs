namespace AlimentaBem.Src.Modules.User.UseCases.ReadList.DTO;

public class UserReadListItemResponse
{
    public Guid userId { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string role { get; set; }
}

public class UserReadListResponse
{
    public List<UserReadListItemResponse> users { get; set; } = new();
}
