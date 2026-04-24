using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public static class DonationStatusTransitionPolicy
{
    private static readonly Dictionary<DonationEnum.DonationStatus, HashSet<DonationEnum.DonationStatus>> AllowedTransitions =
        new()
        {
            [DonationEnum.DonationStatus.Submitted] = new()
            {
                DonationEnum.DonationStatus.InReview,
                DonationEnum.DonationStatus.ReadyForDelivery,
                DonationEnum.DonationStatus.Received,
                DonationEnum.DonationStatus.TemporarilyUnavailable,
            },
            [DonationEnum.DonationStatus.InReview] = new()
            {
                DonationEnum.DonationStatus.ReadyForDelivery,
                DonationEnum.DonationStatus.Received,
                DonationEnum.DonationStatus.TemporarilyUnavailable,
            },
            [DonationEnum.DonationStatus.ReadyForDelivery] = new()
            {
                DonationEnum.DonationStatus.Received,
                DonationEnum.DonationStatus.TemporarilyUnavailable,
            },
            [DonationEnum.DonationStatus.TemporarilyUnavailable] = new()
            {
                DonationEnum.DonationStatus.InReview,
                DonationEnum.DonationStatus.ReadyForDelivery,
            },
        };

    public static bool CanTransition(string? currentStatus, DonationEnum.DonationStatus nextStatus)
    {
        var current = ResolveCurrentStatus(currentStatus);

        if (current == nextStatus)
            return true;

        return AllowedTransitions.TryGetValue(current, out var allowed)
            && allowed.Contains(nextStatus);
    }

    private static DonationEnum.DonationStatus ResolveCurrentStatus(string? value)
    {
        if (DonationStatusParser.TryParseStatus(value, out var status))
            return status;

        // Linhas legadas podem ter status vazio/desconhecido; tratar como estado inicial.
        return DonationEnum.DonationStatus.Submitted;
    }
}
