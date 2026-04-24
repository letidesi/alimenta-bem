using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadOne.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadOne;

public class NaturalPersonReadOneEndPoint : Endpoint<NaturalPersonReadOneRequest, NaturalPersonReadOneResponse, NaturalPersonReadOneMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("natural-person/{userId}");
        Options(u => u.WithTags("naturalPerson"));
        Summary(s =>
        {
            s.Summary = "Read a naturalPerson";
        });
        Roles(EnumRole.Admin.ToString(), EnumRole.Citizen.ToString());
    }

    public override async Task HandleAsync(NaturalPersonReadOneRequest req, CancellationToken ct)
    {
        try
        {
            var naturalPersonReadOneUseCase = new NaturalPersonReadOneUseCase(_context, _localizer);

            var naturalPerson = Map.ToEntity(req);

            var readOneNaturalPerson = await naturalPersonReadOneUseCase.exec(naturalPerson.id);

            var readOnedNaturalPerson = Map.FromEntity(readOneNaturalPerson);

            await SendAsync(readOnedNaturalPerson);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}