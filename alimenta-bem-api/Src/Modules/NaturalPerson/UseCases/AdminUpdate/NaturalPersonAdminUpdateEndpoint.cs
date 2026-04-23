using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpdate;

public class NaturalPersonAdminUpdateEndpoint : Endpoint<NaturalPersonUpdateRequest, NaturalPersonUpdateResponse, NaturalPersonUpdateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Put("natural-person/admin");
        Options(n => n.WithTags("natural-person"));
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update natural person by admin";
            s.Description = "Update donor profile data by admin without changing credentials";
        });
    }

    public override async Task HandleAsync(NaturalPersonUpdateRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new NaturalPersonUpdateUseCase(_context, _localizer);

            var naturalPerson = Map.ToEntity(req);

            var updated = await useCase.exec(naturalPerson);

            var response = Map.FromEntity(updated);

            await SendAsync(response, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
