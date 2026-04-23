namespace AlimentaBem.Src.Modules.Organization.UseCases.Create.DTO;

public class OrganizationCreateRequest
{
    public string name { get; set; }
    public string type { get; set; }
    public string? description { get; set; }
}