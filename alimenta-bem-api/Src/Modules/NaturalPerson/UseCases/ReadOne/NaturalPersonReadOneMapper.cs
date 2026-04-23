using AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadOne.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadOne
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonReadOneMapper : Mapper<NaturalPersonReadOneRequest, NaturalPersonReadOneResponse, NaturalPerson>
    {

        public override NaturalPerson ToEntity(NaturalPersonReadOneRequest req) => new()
        {
            id = req.userId,
        };

        public override NaturalPersonReadOneResponse FromEntity(NaturalPerson n) => new()
        {
            naturalPersonId = n.id,
            name = n.name,
            email = n.emailUser,
            socialName = n.socialName,
            userId = n.userId,
            age = n.age,
            birthdayDate = n.birthdayDate,
            skinColor = n.skinColor.ToString(),
            gender = n.gender.ToString(),
            isPcd = n.isPcd
        };
    }
}