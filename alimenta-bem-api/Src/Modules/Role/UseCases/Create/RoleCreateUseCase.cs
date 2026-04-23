using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Role.Repository;

namespace AlimentaBem.Src.Modules.Role.UseCases.Create
{
    using Role = AlimentaBem.Src.Modules.Role.Repository.Role;

    public class RoleCreateUseCase
    {
        private Localizer _localizer;
        public IRoleData _role_data;
        public RoleCreateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _role_data = new RoleData(context);
            _localizer = localizer;
        }

        public async Task<Role> exec(Role role)
        {
            var createRole = await _role_data.Create(role);

            if (createRole is null) throw new Exception(_localizer["role:RoleCreationFailed"]);

            return createRole;
        }
    }
}