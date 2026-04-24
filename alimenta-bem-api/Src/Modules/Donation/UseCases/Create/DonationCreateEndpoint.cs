using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.Create;

public class DonationCreateEndPoint : Endpoint<DonationCreateRequest, DonationCreateResponse, DonationCreateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("donation");
        Options(n => n.WithTags("donation"));
        Summary(s =>
        {
            s.Summary = "Create a new donation";
            s.Description = "Register a donation on the platform";
        });
        Roles(EnumRole.Citizen.ToString());
    }

    public override async Task HandleAsync(DonationCreateRequest req, CancellationToken ct)
    {
        try
        {
            var donationCreateUseCase = new DonationCreateUseCase(_context, _localizer);

            var donation = Map.ToEntity(req);

            var createDonation = await donationCreateUseCase.exec(donation);

            var createdDonation = Map.FromEntity(createDonation);

            await SendAsync(createdDonation);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}