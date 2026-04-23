using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminDelete.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminDelete;

public class NaturalPersonAdminDeleteEndpoint : Endpoint<NaturalPersonAdminDeleteRequest, NaturalPersonAdminDeleteResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Delete("natural-person/admin/{userId}");
        Options(n => n.WithTags("natural-person"));
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Soft delete a natural person by admin";
            s.Description = "Soft delete a natural person and its user credentials by userId";
        });
    }

    public override async Task HandleAsync(NaturalPersonAdminDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new NaturalPersonAdminDeleteUseCase(_context, _localizer);

            await useCase.exec(req.userId);

            await SendAsync(new NaturalPersonAdminDeleteResponse { success = true }, cancellation: ct);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
