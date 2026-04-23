using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.User.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.Create
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserCreateMapper : Mapper<UserCreateRequest, UserCreateResponse, User>
    {

        public override User ToEntity(UserCreateRequest req) => new()
        {
            name = req.name.Trim(),
            email = req.email.ToLower().Trim(),
            passwordHash = FormatPassword.GenerateHash(req.password),
        };

        public override UserCreateResponse FromEntity(User u) => new()
        {
            id = u.id,
            name = u.name,
            email = u.email,
            createdAt = u.createdAt,
            updatedAt = u.updatedAt
        };
    }
}