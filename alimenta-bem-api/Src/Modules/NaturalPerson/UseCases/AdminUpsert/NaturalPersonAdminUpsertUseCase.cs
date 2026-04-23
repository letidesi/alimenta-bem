using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.Repository;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update;
using AlimentaBem.Src.Modules.Role.Enum;
using AlimentaBem.Src.Modules.User.Repository;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminUpsert
{
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;
    using Role = AlimentaBem.Src.Modules.Role.Repository.Role;
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class NaturalPersonAdminUpsertUseCase
    {
        private readonly Localizer _localizer;
        private readonly IUserData _userData;
        private readonly NaturalPersonUpdateUseCase _naturalPersonUpdateUseCase;
        private readonly isValidEmailFunction _isValidEmailFunction;

        public NaturalPersonAdminUpsertUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _userData = new UserData(context);
            _naturalPersonUpdateUseCase = new NaturalPersonUpdateUseCase(context, localizer);
            _isValidEmailFunction = new isValidEmailFunction();
        }

        public async Task<NaturalPerson> exec(NaturalPerson naturalPerson, string password)
        {
            if (!_isValidEmailFunction.IsValidEmail(naturalPerson.emailUser))
                throw new Exception(_localizer["data:EmailInvalid"]);

            var normalizedEmail = naturalPerson.emailUser.ToLower().Trim();
            var normalizedName = naturalPerson.name.Trim();

            var user = await _userData.ReadOneByEmail(normalizedEmail);

            if (user is null)
            {
                user = new User
                {
                    name = normalizedName,
                    email = normalizedEmail,
                    passwordHash = FormatPassword.GenerateHash(password),
                    roles = new List<Role>
                    {
                        new Role { type = Enum.GetName(EnumRole.Citizen) }
                    }
                };

                user = await _userData.Create(user);
            }
            else
            {
                user.name = normalizedName;
                user.passwordHash = FormatPassword.GenerateHash(password);
                user = await _userData.Update(user);
            }

            naturalPerson.userId = user.id;
            naturalPerson.user = user;
            naturalPerson.emailUser = normalizedEmail;
            naturalPerson.name = normalizedName;

            return await _naturalPersonUpdateUseCase.exec(naturalPerson);
        }
    }
}
