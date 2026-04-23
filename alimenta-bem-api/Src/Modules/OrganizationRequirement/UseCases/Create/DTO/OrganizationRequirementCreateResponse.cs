using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create.DTO;

public class OrganizationRequirementCreateResponse : BaseEntity
{
    public Guid organizationId { get; set; }
    public string itemName { get; set; }
    public int quantity { get; set; }
    public string type { get; set; }
}