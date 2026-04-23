using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert;

public class NaturalPersonAdminUpsertEndpoint : Endpoint<NaturalPersonAdminUpsertRequest, NaturalPersonAdminUpsertResponse, NaturalPersonAdminUpsertMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("natural-person/admin");
        Options(n => n.WithTags("natural-person"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Create or update a natural person by admin";
            s.Description = "Upsert user credentials by email and upsert natural person profile in a single request";
        });
    }

    public override async Task HandleAsync(NaturalPersonAdminUpsertRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new NaturalPersonAdminUpsertUseCase(_context, _localizer);

            var naturalPerson = Map.ToEntity(req);

            var result = await useCase.exec(naturalPerson, req.password);

            var response = Map.FromEntity(result);

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
