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

        public Task<NaturalPerson?> exec(Guid naturalPersonId)
        {
            return _naturalPersonData.ReadNaturalPersonByUser(naturalPersonId);
        }
    }
}