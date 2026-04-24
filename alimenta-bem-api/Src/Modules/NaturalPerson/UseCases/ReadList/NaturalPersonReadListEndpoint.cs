using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadList.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadList;

public class NaturalPersonReadListEndpoint : EndpointWithoutRequest<NaturalPersonReadListResponse, NaturalPersonReadListMapper>
{
    public AlimentaBemContext _context { get; init; }
    public Localizer _localizer { get; init; }

    public override void Configure()
    {
        Get("natural-persons");
        Options(n => n.WithTags("naturalPerson"));
        Summary(s =>
        {
            s.Summary = "Get a list of naturalPersons";
            s.Description = "Retrieve a list of naturalPersons from the platform";
        });
        Roles(EnumRole.Admin.ToString());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var naturalPersonReadListUseCase = new NaturalPersonReadListUseCase(_context, _localizer);

            var naturalPersons = await naturalPersonReadListUseCase.exec();

            var response = Map.FromEntity(naturalPersons);

            await SendAsync(response);
        }
        catch (Exception e)
        {
            ThrowError(e.Message);
        }
    }
}