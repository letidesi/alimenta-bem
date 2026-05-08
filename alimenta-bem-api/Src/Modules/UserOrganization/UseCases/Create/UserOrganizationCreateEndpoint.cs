using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.UserOrganization.UseCases.Create.DTO;

namespace AlimentaBem.Src.Modules.UserOrganization.UseCases.Create;

public class UserOrganizationCreateEndpoint : Endpoint<UserOrganizationCreateRequest, UserOrganizationCreateResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("user-organization");
        Options(n => n.WithTags("user-organization"));
        Roles(EnumRole.Admin.ToString(), EnumRole.Developer.ToString());
        Summary(s =>
        {
            s.Summary = "Link an admin to an organization";
            s.Description = "Only an admin who already belongs to the organization can link another user to it";
        });
    }

    public override async Task HandleAsync(UserOrganizationCreateRequest req, CancellationToken ct)
    {
        try
        {
            var callerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isDeveloper = User.IsInRole(EnumRole.Developer.ToString());

            var useCase = new UserOrganizationCreateUseCase(_context, _localizer);

            var result = await useCase.exec(callerId, req, isDeveloper);

            await SendAsync(new UserOrganizationCreateResponse
            {
                id = result.id,
                userId = result.userId,
                organizationId = result.organizationId,
                createdAt = result.createdAt
            }, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
