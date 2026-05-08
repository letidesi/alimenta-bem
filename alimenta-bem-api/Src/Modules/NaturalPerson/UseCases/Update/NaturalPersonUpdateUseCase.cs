using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.NaturalPerson.Repository;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class NaturalPersonUpdateUseCase
    {
        private Localizer _localizer;
        public IUserData _userData;
        public INaturalPersonData _naturalPersonData;
        private readonly ValidateNaturalPersonData _validateNaturalPersonData;
        public NaturalPersonUpdateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _naturalPersonData = new NaturalPersonData(context);
            _validateNaturalPersonData = new ValidateNaturalPersonData(context, localizer);
        }

        public async Task<NaturalPerson> exec(NaturalPerson naturalPerson)
        {
            var targetNaturalPerson = await _naturalPersonData.CheckNaturalPersonAlreadyExist(naturalPerson);

            if (targetNaturalPerson is null)
            {
                _validateNaturalPersonData.ValidateNaturalPersonFields(naturalPerson);
                return await _naturalPersonData.Create(naturalPerson);
            }

            targetNaturalPerson.name = naturalPerson.name;
            targetNaturalPerson.socialName = naturalPerson.socialName;
            targetNaturalPerson.emailUser = naturalPerson.emailUser;
            targetNaturalPerson.age = naturalPerson.age;
            targetNaturalPerson.birthdayDate = naturalPerson.birthdayDate;
            targetNaturalPerson.skinColor = naturalPerson.skinColor;
            targetNaturalPerson.gender = naturalPerson.gender;
            targetNaturalPerson.isPcd = naturalPerson.isPcd;

            await _naturalPersonData.Update(targetNaturalPerson);

            return targetNaturalPerson;
        }
    }
}