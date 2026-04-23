using AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;

public class Validator : Validator<DonationUpdateStatusRequest>
{
    public Validator()
    {
        RuleFor(request => request.donationId)
            .NotEmpty()
            .WithMessage("Donation id is required");

        RuleFor(request => request.organizationId)
            .NotEmpty()
            .WithMessage("Organization id is required");

        RuleFor(request => request.status)
            .NotEmpty()
            .WithMessage("Status is required");
    }
}
