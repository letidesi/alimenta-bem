using AlimentaBem.EntityMetadata;
using AlimentaBem.Src.Modules.Organization.Repository.Enums;

namespace AlimentaBem.Src.Modules.Organization.Repository
{
    using Donation = AlimentaBem.Src.Modules.Donation.Repository.Donation;
    using OrganizationRequirement = AlimentaBem.Src.Modules.OrganizationRequirement.Repository.OrganizationRequirement;

    public class Organization : BaseEntity
    {
        public virtual ICollection<Donation>? donations { get; set; }
        public virtual ICollection<OrganizationRequirement>? OrganizationRequirements { get; set; }
        public string name { get; set; }
        public TypeOrganization? type { get; set; }
        public string? description { get; set; }
    }
}