using AlimentaBem.Helpers;
using static AlimentaBem.Helpers.InputSanitizer;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.Update.DTO;

public class Validator : Validator<NaturalPersonUpdateRequest>
{
    public Validator(Localizer localizer)
    {
        var _localizer = localizer;

        RuleFor(request => request.name)
         .NotEmpty()
         .WithMessage(_localizer["naturalPerson:nameRequired"])
         .MaximumLength(200)
         .WithMessage(_localizer["naturalPerson:nameRequired"])
         .Must(v => v == null || !ContainsHtml(v))
         .WithMessage("O campo nome não pode conter HTML ou scripts.");

        RuleFor(request => request.socialName)
         .MaximumLength(100)
         .WithMessage(_localizer["naturalPerson:nameRequired"])
         .Must(v => v == null || !ContainsHtml(v))
         .WithMessage("O campo nome social não pode conter HTML ou scripts.")
         .When(r => r.socialName != null);

        RuleFor(request => request.age)
         .NotEmpty()
         .WithMessage(_localizer["naturalPerson:AgeRequired"])
         .MaximumLength(10)
         .WithMessage(_localizer["naturalPerson:AgeRequired"]);

        RuleFor(request => request.birthdayDate)
            .NotEmpty()
            .WithMessage(_localizer["naturalPerson:BirthdayDateRequired"])
            .Must(date => date <= DateOnly.FromDateTime(DateTime.Today) && date >= new DateOnly(1900, 1, 1))
            .WithMessage(_localizer["naturalPerson:BirthdayDateInvalid"]);
    }
}
