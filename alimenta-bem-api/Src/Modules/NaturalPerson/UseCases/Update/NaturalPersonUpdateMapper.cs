using AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update.DTO;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.Repository.Enums;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonUpdateMapper : Mapper<NaturalPersonUpdateRequest, NaturalPersonUpdateResponse, NaturalPerson>
    {

        public override NaturalPerson ToEntity(NaturalPersonUpdateRequest req) => new()
        {
            userId = req.userId,
            emailUser = req.email,
            name = req.name,
            socialName = req.socialName,
            age = req.age,
            birthdayDate = req.birthdayDate,
            gender = EnumHelper.ToEnumOrNull<Gender>(req.gender),
            skinColor = EnumHelper.ToEnumOrNull<SkinColor>(req.skinColor),
            isPcd = req.isPcd
        };

        public override NaturalPersonUpdateResponse FromEntity(NaturalPerson n) => new()
        {
            id = n.id,
            emailUser = n.emailUser,
            name = n.name,
            socialName = n.socialName,
            age = n.age,
            birthdayDate = n.birthdayDate,
            gender = n.gender.ToString(),
            skinColor = n.skinColor.ToString(),
            isPcd = n.isPcd,
            createdAt = n.createdAt,
            updatedAt = n.updatedAt
        };
    }
}