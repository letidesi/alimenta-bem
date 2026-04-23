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
            if (targetNaturalPerson is not null)
            {
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

            _validateNaturalPersonData.ValidateNaturalPersonFields(naturalPerson);
            await _validateNaturalPersonData.ExistDataOfNaturalPerson(naturalPerson);

            var user = await _naturalPersonData.GetUserByEmail(naturalPerson.emailUser);
            if (user is null)
                throw new Exception(_localizer["user:UserNotFound"]);

            naturalPerson.userId = user.id;
            naturalPerson.user = user;

            var updateNaturalPerson = await _naturalPersonData.Create(naturalPerson);
            if (updateNaturalPerson is null)
                throw new Exception(_localizer["naturalPerson:IndividualCreationFailed"]);

            return updateNaturalPerson;
        }
    }
}