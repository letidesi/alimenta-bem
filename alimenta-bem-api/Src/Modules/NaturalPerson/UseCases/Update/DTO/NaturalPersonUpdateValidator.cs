using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update.DTO;

public class Validator : Validator<NaturalPersonUpdateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.name)
         .NotEmpty()
         .WithMessage(_localizer["naturalPerson:nameRequired"]);

        RuleFor(request => request.age)
         .NotEmpty()
         .WithMessage(_localizer["naturalPerson:AgeRequired"]);

        RuleFor(request => request.age)
         .NotEmpty()
         .WithMessage(_localizer["naturalPerson:AgeRequired"]);

        RuleFor(request => request.birthdayDate)
            .NotEmpty()
            .WithMessage(_localizer["naturalPerson:BirthdayDateRequired"]);
    }
}