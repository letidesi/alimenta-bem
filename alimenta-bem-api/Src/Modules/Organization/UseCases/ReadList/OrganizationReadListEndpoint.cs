using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.UseCases.ReadList.DTO;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.ReadList;

public class organizationReadListEndpoint : EndpointWithoutRequest<OrganizationReadListResponse, OrganizationReadListMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("organizations");
        Options(n => n.WithTags("organization"));
        Summary(s =>
        {
            s.Summary = "Get a list of organizations";
            s.Description = "Admins receive only their own organizations; public callers receive all.";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var useCase = new OrganizationReadListUseCase(_context, _localizer);

            IEnumerable<Guid>? filterIds = null;

            var isAdmin = User.IsInRole(EnumRole.Admin.ToString());
            if (isAdmin)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                var userOrgData = new UserOrganizationData(_context);
                filterIds = await userOrgData.GetOrganizationIdsByUser(userId);
            }

            var organizations = await useCase.exec(filterIds);

            var response = Map.FromEntity(organizations);

            await SendAsync(response);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}