using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.UseCases.Create;

public class organizationCreateEndPoint : Endpoint<OrganizationRequirementCreateRequest, OrganizationRequirementCreateResponse, OrganizationRequirementCreateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("organization-requirement");
        Options(n => n.WithTags("organization-requirement"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Create a new organization requirement";
            s.Description = "Register a organization requirement on the platform";
        });
    }

    public override async Task HandleAsync(OrganizationRequirementCreateRequest req, CancellationToken ct)
    {
        try
        {
            var organizationRequirementCreateUseCase = new OrganizationRequirementCreateUseCase(_context, _localizer);

            var organizationRequirement = Map.ToEntity(req);

            var createOrganizationRequirement = await organizationRequirementCreateUseCase.exec(organizationRequirement);

            var createdOrganizationRequirement = Map.FromEntity(createOrganizationRequirement);

            await SendAsync(createdOrganizationRequirement);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}