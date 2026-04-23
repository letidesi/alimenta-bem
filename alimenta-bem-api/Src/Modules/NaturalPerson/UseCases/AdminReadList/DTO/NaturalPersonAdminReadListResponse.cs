namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminReadList.DTO;

public class NaturalPersonAdminReadListResponse
{
    public List<NaturalPersonAdminReadListItem> naturalPersons { get; set; } = new();
}

public class NaturalPersonAdminReadListItem
{
    public Guid id { get; set; }
    public Guid userId { get; set; }
    public string name { get; set; }
    public string? socialName { get; set; }
    public string emailUser { get; set; }
    public string age { get; set; }
    public DateOnly birthdayDate { get; set; }
    public string? gender { get; set; }
    public string? skinColor { get; set; }
    public bool? isPcd { get; set; }
    public int donationCount { get; set; }
}
