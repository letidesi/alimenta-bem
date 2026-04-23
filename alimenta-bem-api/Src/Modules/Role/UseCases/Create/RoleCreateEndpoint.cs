using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.Role.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.Role.UseCases.Create;

public class RoleCreateEndPoint : Endpoint<RoleCreateRequest, RoleCreateResponse, RoleCreateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("role");
        Options(u => u.WithTags("role"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Create a new role";
            s.Description = "Register a role on the platform";
        });
    }

    public override async Task HandleAsync(RoleCreateRequest req, CancellationToken ct)
    {
        try
        {
            var roleCreateUseCase = new RoleCreateUseCase(_context, _localizer);

            var role = Map.ToEntity(req);

            var createRole = await roleCreateUseCase.exec(role);

            var createdRole = Map.FromEntity(createRole);

            await SendAsync(createdRole);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}