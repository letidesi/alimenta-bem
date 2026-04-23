using AlimentaBem.Src.Modules.Role.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.Role.UseCases.Create
{
    using Role = AlimentaBem.Src.Modules.Role.Repository.Role;

    public class RoleCreateMapper : Mapper<RoleCreateRequest, RoleCreateResponse, Role>
    {

        public override Role ToEntity(RoleCreateRequest req) => new()
        {
            userId = req.userId,
            type = req.type
        };

        public override RoleCreateResponse FromEntity(Role r) => new()
        {
            id = r.id,
            userId = r.userId,
            type = r.type,
            createdAt = r.createdAt,
            updatedAt = r.updatedAt
        };
    }
}