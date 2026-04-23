using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.UseCases.ReadList.DTO;

namespace AlimentaBem.Src.Modules.Organization.UseCases.ReadList;

public class organizationReadListEndpoint : EndpointWithoutRequest<OrganizationReadListResponse, OrganizationReadListMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("organizations");
        Options(n => n.WithTags("organization"));
        Summary(s =>
        {
            s.Summary = "Get a list of organizations";
            s.Description = "Retrieve a list of organizations from the platform";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var organizationReadListUseCase = new OrganizationReadListUseCase(_context, _localizer);

            var organizations = await organizationReadListUseCase.exec();

            var response = Map.FromEntity(organizations);

            await SendAsync(response);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}