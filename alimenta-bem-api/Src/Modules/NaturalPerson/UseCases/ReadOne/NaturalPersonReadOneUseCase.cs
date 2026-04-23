using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.Repository;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadOne
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonReadOneUseCase
    {
        private Localizer _localizer;
        public INaturalPersonData _naturalPersonData;
        public NaturalPersonReadOneUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _naturalPersonData = new NaturalPersonData(context);
            _localizer = localizer;
        }

        public async Task<NaturalPerson> exec(Guid naturalPersonId)
        {
            var naturalPerson = await _naturalPersonData.ReadNaturalPersonByUser(naturalPersonId);
            if (naturalPerson is null)
                throw new Exception(_localizer["naturalPerson:NotFoundNaturalPerson"]);

            return naturalPerson;
        }
    }
}