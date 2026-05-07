using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.UseCases.AdminCreate.DTO;

namespace AlimentaBem.Src.Modules.User.UseCases.AdminCreate;

public class UserAdminCreateEndpoint : Endpoint<UserAdminCreateRequest, UserAdminCreateResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("user/admin");
        Options(u => u.WithTags("user"));
        Roles(EnumRole.Admin.ToString(), EnumRole.Developer.ToString());
        Summary(s =>
        {
            s.Summary = "Create a user and link to admin's organizations";
            s.Description = "Creates a new user with the given role and automatically links them to all organizations the requesting admin belongs to.";
        });
    }

    public override async Task HandleAsync(UserAdminCreateRequest req, CancellationToken ct)
    {
        try
        {
            var adminUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isDeveloper = User.IsInRole(EnumRole.Developer.ToString());
            var useCase = new UserAdminCreateUseCase(_context, _localizer);
            var user = await useCase.exec(req.name, req.email, req.password, req.role, adminUserId, req.organizationIds, isDeveloper);

            await SendAsync(new UserAdminCreateResponse
            {
                userId = user.id,
                name = user.name,
                email = user.email,
                role = user.roles?.FirstOrDefault()?.type ?? string.Empty
            }, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
