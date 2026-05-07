using System.Security.Claims;
using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Donation.UseCases.ReadListByOrganization.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.ReadListByOrganization;

public class DonationReadListByOrganizationEndpoint : Endpoint<DonationReadListByOrganizationRequest, DonationReadListByOrganizationResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("donations/organization/{organizationId}");
        Options(n => n.WithTags("donation"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Read donations by organization";
            s.Description = "Retrieve donation queue for one organization";
        });
    }

    public override async Task HandleAsync(DonationReadListByOrganizationRequest req, CancellationToken ct)
    {
        try
        {
            var adminUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await AdminOrganizationGuard.EnsureAccess(_context, adminUserId, req.organizationId, _localizer);

            var useCase = new DonationReadListByOrganizationUseCase(_context);

            var response = await useCase.exec(req.organizationId);

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
