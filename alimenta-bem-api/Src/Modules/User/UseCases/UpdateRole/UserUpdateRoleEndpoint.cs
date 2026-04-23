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
        AllowAnonymous();
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
                role = updatedUser.roles?.FirstOrDefault()?.type ?? string.Empty
            };

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
