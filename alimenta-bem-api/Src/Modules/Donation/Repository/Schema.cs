using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.Donation.Repository
{
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
    }
}