using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminReadList.DTO;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminReadList;

public class NaturalPersonAdminReadListEndpoint : EndpointWithoutRequest<NaturalPersonAdminReadListResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("natural-persons/admin");
        Options(n => n.WithTags("natural-person"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Read all natural persons for admin";
            s.Description = "Retrieve donors who donated to the admin's organizations";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var adminUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userOrgData = new UserOrganizationData(_context);
            var orgIds = await userOrgData.GetOrganizationIdsByUser(adminUserId);

            var useCase = new NaturalPersonAdminReadListUseCase(_context);

            var response = await useCase.exec(orgIds);

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
