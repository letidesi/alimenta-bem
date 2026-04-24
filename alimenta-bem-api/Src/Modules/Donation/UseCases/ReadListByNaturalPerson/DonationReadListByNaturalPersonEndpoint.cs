using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.UseCases.ReadListByNaturalPerson.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.ReadListByNaturalPerson;

public class DonationReadListByNaturalPersonEndpoint : Endpoint<DonationReadListByNaturalPersonRequest, DonationReadListByNaturalPersonResponse>
{
    public AlimentaBemContext _context { get; init; }

    public override void Configure()
    {
        Get("donations/natural-person/{naturalPersonId}");
        Options(n => n.WithTags("donation"));
        Roles(EnumRole.Citizen.ToString(), EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Read donations by natural person";
            s.Description = "Retrieve donor donation history with current status";
        });
    }

    public override async Task HandleAsync(DonationReadListByNaturalPersonRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new DonationReadListByNaturalPersonUseCase(_context);

            var response = await useCase.exec(req.naturalPersonId);

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
