using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.Repository.Enums;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonAdminUpsertMapper : Mapper<NaturalPersonAdminUpsertRequest, NaturalPersonAdminUpsertResponse, NaturalPerson>
    {
        public override NaturalPerson ToEntity(NaturalPersonAdminUpsertRequest req) => new()
        {
            emailUser = req.email.ToLower().Trim(),
            name = req.name.Trim(),
            socialName = req.socialName,
            age = req.age,
            birthdayDate = req.birthdayDate,
            gender = EnumHelper.ToEnumOrNull<Gender>(req.gender),
            skinColor = EnumHelper.ToEnumOrNull<SkinColor>(req.skinColor),
            isPcd = req.isPcd
        };

        public override NaturalPersonAdminUpsertResponse FromEntity(NaturalPerson n) => new()
        {
            id = n.id,
            userId = n.userId,
            name = n.name,
            emailUser = n.emailUser,
            socialName = n.socialName,
            age = n.age,
            birthdayDate = n.birthdayDate,
            gender = n.gender?.ToString(),
            skinColor = n.skinColor?.ToString(),
            isPcd = n.isPcd,
            createdAt = n.createdAt,
            updatedAt = n.updatedAt
        };
    }
}
