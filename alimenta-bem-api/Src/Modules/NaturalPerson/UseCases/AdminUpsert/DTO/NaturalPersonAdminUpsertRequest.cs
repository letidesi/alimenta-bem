namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert.DTO;

public class NaturalPersonAdminUpsertRequest
{
    public string email { get; set; }
    public string password { get; set; }
    public string name { get; set; }
    public string? socialName { get; set; }
    public string age { get; set; }
    public DateOnly birthdayDate { get; set; }
    public string? gender { get; set; }
    public string? skinColor { get; set; }
    public bool? isPcd { get; set; }
}
