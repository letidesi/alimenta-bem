using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Update.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Update;

public class OrganizationRequirementUpdateEndpoint : Endpoint<OrganizationRequirementUpdateRequest, OrganizationRequirementUpdateResponse, OrganizationRequirementUpdateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Put("organization-requirement");
        Options(n => n.WithTags("organization-requirement"));
        Roles(EnumRole.Admin.ToString());
    }

    public override async Task HandleAsync(OrganizationRequirementUpdateRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new OrganizationRequirementUpdateUseCase(_context, _localizer);

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
