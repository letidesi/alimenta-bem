using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public class DonationUpdateStatusEndpoint : Endpoint<DonationUpdateStatusRequest, DonationUpdateStatusResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Put("donation/status");
        Options(n => n.WithTags("donation"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Update donation status";
            s.Description = "Update donation queue status for the selected organization";
        });
    }

    public override async Task HandleAsync(DonationUpdateStatusRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new DonationUpdateStatusUseCase(_context, _localizer);

            var response = await useCase.exec(req);

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
