using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.Donation.Repository
{
    using DonationEnum = AlimentaBem.Src.Modules.Donation.Enum;
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class Donation : BaseEntity
    {
        public Guid naturalPersonId { get; set; }
        public virtual NaturalPerson? naturalPerson { get; set; }
        public Guid organizationId { get; set; }
        public virtual Organization? organization { get; set; }
        public string itemName { get; set; }
        public int amountDonated { get; set; }
        public string status { get; set; } = DonationEnum.DonationStatus.Submitted.ToString();
        public string? unavailableReason { get; set; }
        public string? unavailableMessage { get; set; }
        public DateTimeOffset? reviewedAt { get; set; }
        public DateTimeOffset? receivedAt { get; set; }
    }
}