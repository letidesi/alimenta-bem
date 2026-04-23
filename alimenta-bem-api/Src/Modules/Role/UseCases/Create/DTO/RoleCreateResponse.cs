using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.Role.UseCases.Create.DTO;

public class RoleCreateResponse : BaseEntity
{
    public Guid userId { get; set; }
    public string type { get; set; }
}