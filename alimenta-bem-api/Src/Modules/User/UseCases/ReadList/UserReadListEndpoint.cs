using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.UseCases.ReadList.DTO;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.ReadList;

public class UserReadListEndpoint : EndpointWithoutRequest<UserReadListResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("users");
        Options(u => u.WithTags("user"));
        Roles(EnumRole.Admin.ToString(), EnumRole.Developer.ToString());
        Summary(s =>
        {
            s.Summary = "Read all users";
            s.Description = "Admins see only users linked to their organizations; developers see all.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var useCase = new UserReadListUseCase(_context, _localizer);

            IEnumerable<Guid>? filterOrgIds = null;
            if (User.IsInRole(EnumRole.Admin.ToString()) && !User.IsInRole(EnumRole.Developer.ToString()))
            {
                var adminUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userOrgData = new UserOrganizationData(_context);
                filterOrgIds = await userOrgData.GetOrganizationIdsByUser(adminUserId);
            }

            var users = await useCase.exec(filterOrgIds);

            var response = new UserReadListResponse
            {
                users = users.Select(user => new UserReadListItemResponse
                {
                    userId = user.id,
                    name = user.name,
                    email = user.email,
                    role = ResolveCurrentRole(user.roles)
                }).ToList()
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
