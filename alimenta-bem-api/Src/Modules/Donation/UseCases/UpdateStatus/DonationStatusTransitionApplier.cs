using AlimentaBem.Helpers;
using DonationEntity = AlimentaBem.Src.Modules.Donation.Repository.Donation;
using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public static class DonationStatusTransitionApplier
{
    public static void Apply(DonationEntity donation, DonationEnum.DonationStatus nextStatus, string? unavailableReasonValue, Localizer localizer)
    {
        donation.status = nextStatus.ToString();
        donation.updatedAt = DateTimeOffset.UtcNow;
        donation.reviewedAt = nextStatus == DonationEnum.DonationStatus.Submitted
            ? null
            : donation.reviewedAt ?? DateTimeOffset.UtcNow;

        ApplySideEffects(donation, nextStatus, unavailableReasonValue, localizer);
    }

    private static void ApplySideEffects(DonationEntity donation, DonationEnum.DonationStatus nextStatus, string? unavailableReasonValue, Localizer localizer)
    {
        switch (nextStatus)
        {
            case DonationEnum.DonationStatus.Submitted:
                donation.reviewedAt = null;
                donation.receivedAt = null;
                donation.unavailableReason = null;
                donation.unavailableMessage = null;
                break;

            case DonationEnum.DonationStatus.Received:
                donation.receivedAt = DateTimeOffset.UtcNow;
                donation.unavailableReason = null;
                donation.unavailableMessage = null;
                break;

            case DonationEnum.DonationStatus.TemporarilyUnavailable:
                if (!DonationStatusParser.TryParseUnavailableReason(unavailableReasonValue, out var reason))
                    throw new Exception(localizer["donation:InvalidUnavailableReason"]);

                donation.unavailableReason = reason.ToString();
                donation.unavailableMessage = DonationUnavailableMessageHelper.BuildForCitizen(reason, localizer);
                donation.receivedAt = null;
                break;

            case DonationEnum.DonationStatus.InReview:
            case DonationEnum.DonationStatus.ReadyForDelivery:
                donation.unavailableReason = null;
                donation.unavailableMessage = null;
                donation.receivedAt = null;
                break;
        }
    }
}
