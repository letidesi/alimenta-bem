using AlimentaBem.EntityMetadata;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository.Enums;

namespace AlimentaBem.Src.Modules.OrganizationRequirement.Repository
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

    public class OrganizationRequirement : BaseEntity
    {
        public Guid organizationId { get; set; }
        public Organization? organization { get; set; }
        public string itemName { get; set; }
        public int quantity { get; set; }
        public Priority type { get; set; }
    }
}