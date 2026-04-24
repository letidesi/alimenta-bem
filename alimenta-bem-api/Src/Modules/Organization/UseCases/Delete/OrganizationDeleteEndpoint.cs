using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.UseCases.Delete.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Delete;

public class OrganizationDeleteEndpoint : Endpoint<OrganizationDeleteRequest, OrganizationDeleteResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Delete("organization/{id}");
        Options(n => n.WithTags("organization"));
        Roles(EnumRole.Admin.ToString());
    }

    public override async Task HandleAsync(OrganizationDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new OrganizationDeleteUseCase(_context, _localizer);

            await useCase.exec(req.id);

            await SendAsync(new OrganizationDeleteResponse { success = true });
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
