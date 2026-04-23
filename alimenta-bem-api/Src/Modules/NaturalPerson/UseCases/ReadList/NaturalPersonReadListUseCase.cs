using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.NaturalPerson.Repository;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.ReadList
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonReadListUseCase
    {
        private Localizer _localizer;
        public IUserData _userData;
        public INaturalPersonData _naturalPersonData;

        public NaturalPersonReadListUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _naturalPersonData = new NaturalPersonData(context);
        }

        public async Task<List<NaturalPerson>> exec()
        {

            var naturalPersons = await _naturalPersonData.ReadList();
            if (naturalPersons.Count == 0)
                throw new Exception(_localizer["naturalPerson:NotFoundNaturalPerson"]);

            return naturalPersons;
        }
    }
}