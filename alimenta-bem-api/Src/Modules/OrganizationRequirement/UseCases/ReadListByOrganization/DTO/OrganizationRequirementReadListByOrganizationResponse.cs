namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.ReadListByOrganization.DTO;

public class OrganizationRequirementReadListByOrganizationResponse
{
    public List<OrganizationRequirementReadListByOrganizationItem> requirements { get; set; } = new();

    public class OrganizationRequirementReadListByOrganizationItem
    {
        public Guid id { get; set; }
        public string itemName { get; set; }
        public int quantity { get; set; }
        public string type { get; set; }
    }
}
