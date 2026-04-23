namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create.DTO;

public class OrganizationRequirementCreateRequest
{
    public Guid organizationId { get; set; }
    public string itemName { get; set; }
    public int quantity { get; set; }
    public string type { get; set; }
}