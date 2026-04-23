using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.Role.Repository
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class Role : BaseEntity
    {
        public Guid userId { get; set; }
        public virtual User? user { get; set; }
        public string type { get; set; }
    }
}