namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadList.DTO;

public class NaturalPersonReadListResponse
{
    public List<NaturalPersonReadListItem> naturalPersons { get; set; }

    public class NaturalPersonReadListItem
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}