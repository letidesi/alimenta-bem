using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public static class DonationStatusParser
{
    private static readonly Dictionary<string, DonationEnum.DonationStatus> StatusAliases =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Submitted"] = DonationEnum.DonationStatus.Submitted,
            ["Sent"] = DonationEnum.DonationStatus.Submitted,
            ["Enviada"] = DonationEnum.DonationStatus.Submitted,

            ["InReview"] = DonationEnum.DonationStatus.InReview,
            ["UnderReview"] = DonationEnum.DonationStatus.InReview,
            ["EmAnalise"] = DonationEnum.DonationStatus.InReview,

            ["ReadyForDelivery"] = DonationEnum.DonationStatus.ReadyForDelivery,
            ["AwaitingDelivery"] = DonationEnum.DonationStatus.ReadyForDelivery,
            ["AguardandoEntrega"] = DonationEnum.DonationStatus.ReadyForDelivery,

            ["Received"] = DonationEnum.DonationStatus.Received,
            ["Recebida"] = DonationEnum.DonationStatus.Received,

            ["TemporarilyUnavailable"] = DonationEnum.DonationStatus.TemporarilyUnavailable,
            ["UnavailableAtTheMoment"] = DonationEnum.DonationStatus.TemporarilyUnavailable,
            ["IndisponivelNoMomento"] = DonationEnum.DonationStatus.TemporarilyUnavailable,
        };

    private static readonly Dictionary<string, DonationEnum.DonationUnavailableReason> UnavailableReasonAliases =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["ReceivingInstability"] = DonationEnum.DonationUnavailableReason.ReceivingInstability,
            ["InstabilidadeRecebimento"] = DonationEnum.DonationUnavailableReason.ReceivingInstability,

            ["NeedAlreadyMet"] = DonationEnum.DonationUnavailableReason.NeedAlreadyMet,
            ["NecessidadeJaAtendida"] = DonationEnum.DonationUnavailableReason.NeedAlreadyMet,

            ["OfferNeedsAdjustment"] = DonationEnum.DonationUnavailableReason.OfferNeedsAdjustment,
            ["OfertaPrecisaAjuste"] = DonationEnum.DonationUnavailableReason.OfferNeedsAdjustment,
        };

    public static bool TryParseStatus(string? value, out DonationEnum.DonationStatus status)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            status = default;
            return false;
        }

        if (System.Enum.TryParse(value, ignoreCase: true, out status))
            return true;

        return StatusAliases.TryGetValue(value, out status);
    }

    public static bool TryParseUnavailableReason(string? value, out DonationEnum.DonationUnavailableReason reason)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            reason = default;
            return false;
        }

        if (System.Enum.TryParse(value, ignoreCase: true, out reason))
            return true;

        return UnavailableReasonAliases.TryGetValue(value, out reason);
    }
}
