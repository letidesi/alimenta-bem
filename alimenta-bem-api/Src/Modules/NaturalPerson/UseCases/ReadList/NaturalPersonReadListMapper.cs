using AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadList.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadList
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonReadListMapper : ResponseMapper<NaturalPersonReadListResponse, List<NaturalPerson>>
    {
        public override NaturalPersonReadListResponse FromEntity(List<NaturalPerson> list) => new()
        {
            naturalPersons = list.Select(n => new NaturalPersonReadListResponse.NaturalPersonReadListItem()
            {
                id = n.id,
                name = n.name
            }).ToList()
        };
    }
}