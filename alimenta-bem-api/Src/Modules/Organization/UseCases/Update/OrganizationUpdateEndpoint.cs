using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.UseCases.Update.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Update;

public class OrganizationUpdateEndpoint : Endpoint<OrganizationUpdateRequest, OrganizationUpdateResponse, OrganizationUpdateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Put("organization");
        Options(n => n.WithTags("organization"));
        Roles(EnumRole.Admin.ToString());
    }

    public override async Task HandleAsync(OrganizationUpdateRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new OrganizationUpdateUseCase(_context, _localizer);

            var entity = Map.ToEntity(req);

            var updated = await useCase.exec(entity);

            await SendAsync(Map.FromEntity(updated));
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
