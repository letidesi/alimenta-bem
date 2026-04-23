using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.User.UseCases.Create.DTO;

public class UserCreateResponse : BaseEntity
{
    public string name { get; set; }
    public string email { get; set; }
    public string passwordHash { get; set; }
}