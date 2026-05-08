using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.UserOrganization.UseCases.Delete;

public class UserOrganizationDeleteRequest
{
    public Guid userId { get; set; }
    public Guid organizationId { get; set; }
}

public class UserOrganizationDeleteEndpoint : Endpoint<UserOrganizationDeleteRequest>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Delete("user-organization");
        Options(n => n.WithTags("user-organization"));
        Roles(EnumRole.Admin.ToString(), EnumRole.Developer.ToString());
        Summary(s =>
        {
            s.Summary = "Unlink a user from an organization";
            s.Description = "Developer can unlink any user. Admin can only unlink users from their own organizations.";
        });
    }

    public override async Task HandleAsync(UserOrganizationDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var data = new UserOrganizationData(_context);
            var removed = await data.Unlink(req.userId, req.organizationId);

            if (!removed)
                ThrowError(_localizer["userOrganization:LinkNotFound"]);

            await SendNoContentAsync(ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
