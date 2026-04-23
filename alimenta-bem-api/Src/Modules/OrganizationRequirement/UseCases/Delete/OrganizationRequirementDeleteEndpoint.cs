using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Delete.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Delete;

public class OrganizationRequirementDeleteEndpoint : Endpoint<OrganizationRequirementDeleteRequest, OrganizationRequirementDeleteResponse>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Delete("organization-requirement/{id}");
        Options(n => n.WithTags("organization-requirement"));
        Roles(EnumRole.Admin.ToString());
    }

    public override async Task HandleAsync(OrganizationRequirementDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var useCase = new OrganizationRequirementDeleteUseCase(_context, _localizer);
            await useCase.exec(req.id);
            await SendAsync(new OrganizationRequirementDeleteResponse { success = true });
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}
