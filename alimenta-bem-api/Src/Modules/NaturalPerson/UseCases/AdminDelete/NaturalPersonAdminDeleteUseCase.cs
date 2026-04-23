using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.Repository;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminDelete;

public class NaturalPersonAdminDeleteUseCase
{
    private readonly Localizer _localizer;
    private readonly INaturalPersonData _naturalPersonData;

    public NaturalPersonAdminDeleteUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _localizer = localizer;
        _naturalPersonData = new NaturalPersonData(context);
    }

    public async Task exec(Guid userId)
    {
        var naturalPerson = await _naturalPersonData.ReadOneByUserId(userId);

        if (naturalPerson is null)
            throw new Exception(_localizer["naturalPerson:NotFoundNaturalPerson"]);

        await _naturalPersonData.SoftDelete(naturalPerson);
    }
}
