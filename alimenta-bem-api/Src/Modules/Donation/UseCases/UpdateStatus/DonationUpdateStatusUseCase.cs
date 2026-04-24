using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Donation.Repository;
using AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public class DonationUpdateStatusUseCase
{
    private readonly IDonationData _donationData;
    private readonly Localizer _localizer;

    public DonationUpdateStatusUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _donationData = new DonationData(context);
        _localizer = localizer;
    }

    public async Task<DonationUpdateStatusResponse> exec(DonationUpdateStatusRequest request)
    {
        var donation = await _donationData.ReadOneById(request.donationId)
            ?? throw new Exception(_localizer["donation:NotFound"]);

        if (donation.organizationId != request.organizationId)
            throw new Exception(_localizer["donation:OrganizationMismatch"]);

        if (!DonationStatusParser.TryParseStatus(request.status, out var nextStatus))
            throw new Exception(_localizer["donation:InvalidStatus"]);

        if (!DonationStatusTransitionPolicy.CanTransition(donation.status, nextStatus))
            throw new Exception(_localizer["donation:InvalidStatusTransition"]);

        DonationStatusTransitionApplier.Apply(donation, nextStatus, request.unavailableReason, _localizer);

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
}
