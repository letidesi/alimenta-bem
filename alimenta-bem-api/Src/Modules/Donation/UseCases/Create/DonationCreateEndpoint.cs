using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;

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
            s.Summary = "Create a new natural person";
            s.Description = "Register a natural person on the platform";
        });
        AllowAnonymous();
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