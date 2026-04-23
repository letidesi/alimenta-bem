using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update.DTO;

public class NaturalPersonUpdateResponse : BaseEntity
{
    public string name { get; set; }
    public string? socialName { get; set; }
    public string emailUser { get; set; }
    public string age { get; set; }
    public DateOnly birthdayDate { get; set; }
    public string? gender { get; set; }
    public string? skinColor { get; set; }
    public bool? isPcd { get; set; }
}