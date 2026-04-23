namespace AlimentaBem.Src.Modules.Organization.UseCases.ReadList.DTO;

public class OrganizationReadListResponse
{
    public List<OrganizationReadListItem> organizations { get; set; }

    public class OrganizationReadListItem
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}