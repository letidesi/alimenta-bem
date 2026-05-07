using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.UserOrganization.Repository
{
    using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class UserOrganization : BaseEntity
    {
        public Guid userId { get; set; }
        public virtual User? user { get; set; }
        public Guid organizationId { get; set; }
        public virtual Organization? organization { get; set; }
    }
}
