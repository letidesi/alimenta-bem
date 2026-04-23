using AlimentaBem.Helpers;
using AlimentaBem.Context;
using AlimentaBem.Src.Modules.Donation.Repository;

namespace AlimentaBem.Src.Modules.Donation.UseCases.Create
{
    using Donation = AlimentaBem.Src.Modules.Donation.Repository.Donation;

    public class DonationCreateUseCase
    {
        private Localizer _localizer;
        public IDonationData _donationData;
        public DonationCreateUseCase(AlimentaBemContext context, Localizer localizer)
        {
            _localizer = localizer;
            _donationData = new DonationData(context);
        }

        public async Task<Donation> exec(Donation donation)
        {
            var createDonation = await _donationData.Create(donation);
            if (createDonation is null)
                throw new Exception(_localizer["donation:IndividualCreationFailed"]);

            return createDonation;
        }
    }
}