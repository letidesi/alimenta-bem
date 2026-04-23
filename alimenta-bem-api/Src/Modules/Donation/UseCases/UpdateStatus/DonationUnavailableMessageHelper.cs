using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;

namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus;

public static class DonationUnavailableMessageHelper
{
    public static string BuildForCitizen(DonationEnum.DonationUnavailableReason reason)
    {
        if (reason == DonationEnum.DonationUnavailableReason.NeedAlreadyMet)
        {
            return "Obrigado pela sua doação. Esta necessidade já foi atendida no momento. Para não atrasar sua ajuda, escolha outra instituição ativa.";
        }

        if (reason == DonationEnum.DonationUnavailableReason.OfferNeedsAdjustment)
        {
            return "Obrigado pela sua doação. Um pequeno ajuste de item ou quantidade pode acelerar o recebimento. Para não atrasar sua ajuda, escolha outra instituição ativa.";
        }

        return "Obrigado pela sua doação. Esta instituição está com recebimento temporariamente instável. Para não atrasar sua ajuda, escolha outra instituição ativa.";
    }
}
