using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.UseCases.UpdateRole.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.UpdateRole;

public class UserUpdateRoleEndpoint : Endpoint<UserUpdateRoleRequest, UserUpdateRoleResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Put("user/role");
        Options(u => u.WithTags("user"));
        Roles(EnumRole.Admin.ToString(), EnumRole.Developer.ToString());
        Summary(s =>
        {
            s.Summary = "Update user role";
            s.Description = "Change the role of a user to one of the existing roles";
        });
    }

    public override async Task HandleAsync(UserUpdateRoleRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new UserUpdateRoleUseCase(_context, _localizer);
            var updatedUser = await useCase.exec(req.userId, req.role);

            var response = new UserUpdateRoleResponse
            {
                userId = updatedUser.id,
                name = updatedUser.name,
                email = updatedUser.email,
                role = ResolveCurrentRole(updatedUser.roles)
            };

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }

    private static string ResolveCurrentRole(ICollection<AlimentaBem.Src.Modules.Role.Repository.Role>? roles)
    {
        if (roles is null || roles.Count == 0)
            return string.Empty;

        return roles
            .OrderByDescending(role => GetRolePriority(role.type))
            .ThenByDescending(role => role.createdAt)
            .Select(role => role.type)
            .FirstOrDefault() ?? string.Empty;
    }

    private static int GetRolePriority(string role)
    {
        return role switch
        {
            nameof(EnumRole.Admin) => 3,
            nameof(EnumRole.Developer) => 2,
            nameof(EnumRole.Citizen) => 1,
            _ => 0,
        };
    }
}
