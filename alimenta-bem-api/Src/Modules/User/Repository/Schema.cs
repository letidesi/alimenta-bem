using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Src.Modules.User.Repository
{
    using Role = AlimentaBem.Src.Modules.Role.Repository.Role;
    using NaturalPerson = AlimentaBem.Src.Modules.NaturalPerson.Repository.NaturalPerson;

    public class User : BaseEntity
    {
        public virtual ICollection<Role>? roles { get; set; }
        public virtual ICollection<NaturalPerson>? naturalPersons { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
    }
}