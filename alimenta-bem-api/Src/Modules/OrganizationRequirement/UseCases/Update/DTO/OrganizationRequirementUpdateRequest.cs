namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Update.DTO;

public class OrganizationRequirementUpdateRequest
{
    public Guid id { get; set; }
    public Guid organizationId { get; set; }
    public string itemName { get; set; }
    public int quantity { get; set; }
    public string type { get; set; }
}
