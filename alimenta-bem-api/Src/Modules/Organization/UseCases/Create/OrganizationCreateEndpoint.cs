using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.UseCases.Create.DTO;
using AlimentaBem.Src.Modules.Role.Enum;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Create;

public class OrganizationEndPoint : Endpoint<OrganizationCreateRequest, OrganizationCreateResponse, OrganizationCreateMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Post("organization");
        Options(n => n.WithTags("organization"));
        Roles(EnumRole.Admin.ToString());
        Summary(s =>
        {
            s.Summary = "Create a new organization";
            s.Description = "Register a organization on the platform";
        });
    }

    public override async Task HandleAsync(OrganizationCreateRequest req, CancellationToken ct)
    {
        try
        {
            var organizationCreateUseCase = new OrganizationCreateUseCase(_context, _localizer);

            var organization = Map.ToEntity(req);

            var createOrganization = await organizationCreateUseCase.exec(organization);

            var createdOrganization = Map.FromEntity(createOrganization);

            await SendAsync(createdOrganization);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}