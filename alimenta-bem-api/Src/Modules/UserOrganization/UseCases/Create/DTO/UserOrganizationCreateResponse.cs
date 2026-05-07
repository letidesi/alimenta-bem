namespace AlimentaBem.Src.Modules.UserOrganization.UseCases.Create.DTO;

public class UserOrganizationCreateResponse
{
    public Guid id { get; set; }
    public Guid userId { get; set; }
    public Guid organizationId { get; set; }
    public DateTimeOffset createdAt { get; set; }
}
