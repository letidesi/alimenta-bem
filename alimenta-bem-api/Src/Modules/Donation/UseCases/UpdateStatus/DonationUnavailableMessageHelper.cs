using AlimentaBem.Helpers;
using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public static class DonationUnavailableMessageHelper
{
    public static string BuildForCitizen(DonationEnum.DonationUnavailableReason reason, Localizer localizer)
    {
        var key = reason switch
        {
            DonationEnum.DonationUnavailableReason.NeedAlreadyMet => "donation:UnavailableMessageNeedAlreadyMet",
            DonationEnum.DonationUnavailableReason.OfferNeedsAdjustment => "donation:UnavailableMessageOfferNeedsAdjustment",
            _ => "donation:UnavailableMessageReceivingInstability",
        };

        return localizer[key];
    }
}
