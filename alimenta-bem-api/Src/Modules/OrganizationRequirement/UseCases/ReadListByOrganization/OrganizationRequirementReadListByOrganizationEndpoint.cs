using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.ReadListByOrganization.DTO;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.ReadListByOrganization;

public class OrganizationRequirementReadListByOrganizationEndpoint : Endpoint<OrganizationRequirementReadListByOrganizationRequest, OrganizationRequirementReadListByOrganizationResponse, OrganizationRequirementReadListByOrganizationMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("organization-requirements/{organizationId}");
        Options(n => n.WithTags("organization-requirement"));
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get requirements for one organization";
            s.Description = "Retrieve donation requirements by organization id";
        });
    }

    public override async Task HandleAsync(OrganizationRequirementReadListByOrganizationRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new OrganizationRequirementReadListByOrganizationUseCase(_context, _localizer);
            var requirements = await useCase.exec(req.organizationId);
            var response = Map.FromEntity(requirements);
            await SendAsync(response);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
