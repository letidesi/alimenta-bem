using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.Repository;
using AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;
using DonationEntity = AlimentaBem.Src.Modules.Donation.Repository.Donation;
using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public class DonationUpdateStatusUseCase
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

    private readonly IDonationData _donationData;

    public DonationUpdateStatusUseCase(AlimentaBemContext context)
    {
        _donationData = new DonationData(context);
    }

    public async Task<DonationUpdateStatusResponse> exec(DonationUpdateStatusRequest request)
    {
        var donation = await _donationData.ReadOneById(request.donationId);

        if (donation is null)
            throw new Exception("Doação não encontrada.");

        if (donation.organizationId != request.organizationId)
            throw new Exception("A doação não pertence à instituição selecionada.");

        if (!TryParseStatus(request.status, out var nextStatus))
            throw new Exception("Status de doação inválido.");

        if (!CanTransition(donation.status, nextStatus))
            throw new Exception("Transição de status inválida para esta doação.");

        donation.status = nextStatus.ToString();
        donation.updatedAt = DateTimeOffset.UtcNow;
        donation.reviewedAt = nextStatus == DonationEnum.DonationStatus.Submitted
            ? null
            : donation.reviewedAt ?? DateTimeOffset.UtcNow;

        ApplyStatusSideEffects(donation, nextStatus, request.unavailableReason);

        var updated = await _donationData.Update(donation);

        return new DonationUpdateStatusResponse
        {
            id = updated.id,
            naturalPersonId = updated.naturalPersonId,
            organizationId = updated.organizationId,
            itemName = updated.itemName,
            amountDonated = updated.amountDonated,
            status = updated.status,
            unavailableReason = updated.unavailableReason,
            unavailableMessage = updated.unavailableMessage,
            reviewedAt = updated.reviewedAt,
            receivedAt = updated.receivedAt,
            updatedAt = updated.updatedAt,
        };
    }

    private static bool CanTransition(string currentStatus, DonationEnum.DonationStatus nextStatus)
    {
        var current = ResolveCurrentStatus(currentStatus);

        if (current == nextStatus)
            return true;

        return AllowedTransitions.TryGetValue(current, out var allowed)
            && allowed.Contains(nextStatus);
    }

    private static DonationEnum.DonationStatus ResolveCurrentStatus(string? currentStatus)
    {
        if (TryParseStatus(currentStatus, out var parsedStatus))
            return parsedStatus;

        // Legacy rows can have empty/unknown status; treat them as initial state.
        return DonationEnum.DonationStatus.Submitted;
    }

    private static bool TryParseStatus(string? value, out DonationEnum.DonationStatus status)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            status = default;
            return false;
        }

        if (System.Enum.TryParse(value, true, out status))
            return true;

        return StatusAliases.TryGetValue(value, out status);
    }

    private static void ApplyStatusSideEffects(
        DonationEntity donation,
        DonationEnum.DonationStatus nextStatus,
        string? unavailableReasonValue)
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
                if (!TryParseUnavailableReason(unavailableReasonValue, out var reason))
                    throw new Exception("Selecione um motivo válido para indisponibilidade.");

                donation.unavailableReason = reason.ToString();
                donation.unavailableMessage = DonationUnavailableMessageHelper.BuildForCitizen(reason);
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

    private static bool TryParseUnavailableReason(string? value, out DonationEnum.DonationUnavailableReason reason)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            reason = default;
            return false;
        }

        if (System.Enum.TryParse(value, true, out reason))
            return true;

        return UnavailableReasonAliases.TryGetValue(value, out reason);
    }
}
