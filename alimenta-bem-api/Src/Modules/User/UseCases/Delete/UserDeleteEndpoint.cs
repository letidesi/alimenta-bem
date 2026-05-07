using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.User.UseCases.Delete.DTO;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.User.UseCases.Delete;

public class UserDeleteEndpoint : Endpoint<UserDeleteRequest>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Delete("user/{userId}");
        Options(u => u.WithTags("user"));
        Roles(EnumRole.Admin.ToString(), EnumRole.Developer.ToString());
        Summary(s =>
        {
            s.Summary = "Delete a user";
            s.Description = "Developer can delete any user. Admin can only delete users linked to their organizations.";
        });
    }

    public override async Task HandleAsync(UserDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var callerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isDeveloper = User.IsInRole(EnumRole.Developer.ToString());

            if (callerId == req.userId)
                ThrowError(_localizer["user:CannotDeleteSelf"]);

            if (!isDeveloper)
            {
                var userOrgData = new UserOrganizationData(_context);
                var adminOrgIds = await userOrgData.GetOrganizationIdsByUser(callerId);
                var targetOrgIds = await userOrgData.GetOrganizationIdsByUser(req.userId);
                var hasSharedOrg = adminOrgIds.Any(id => targetOrgIds.Contains(id));
                if (!hasSharedOrg)
                    ThrowError(_localizer["user:NoAccess"]);
            }

            var userData = new UserData(_context);
            var deleted = await userData.Delete(req.userId);

            if (!deleted)
                ThrowError(_localizer["user:UserNotFound"]);

            await SendNoContentAsync(ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
