namespace AlimentaBem.Src.Modules.Organization.UseCases.Update.DTO;

public class OrganizationUpdateResponse
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string? type { get; set; }
    public string? description { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset? updatedAt { get; set; }
}
