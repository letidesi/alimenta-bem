using AlimentaBem.Src.Modules.User.UseCases.ReadOne.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.ReadOne
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserReadOneMapper : Mapper<UserReadOneRequest, UserReadOneResponse, User>
    {

        public override User ToEntity(UserReadOneRequest req) => new()
        {
            id = req.userId,
        };

        public override UserReadOneResponse FromEntity(User u) => new()
        {
            userId = u.id,
            name = u.name,
            email = u.email,
            password = u.passwordHash,
        };
    }
}