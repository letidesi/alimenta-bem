using AlimentaBem.EntityMetadata;
using AlimentaBem.Src.Modules.NaturalPerson.Repository.Enums;

namespace AlimentaBem.Src.Modules.NaturalPerson.Repository
{
    using Donation = AlimentaBem.Src.Modules.Donation.Repository.Donation;
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public class NaturalPerson : BaseEntity
    {
        public virtual ICollection<Donation>? donations { get; set; }
        public Guid userId { get; set; }
        public User? user { get; set; }
        public string name { get; set; }
        public string emailUser { get; set; }
        public string? socialName { get; set; }
        public string age { get; set; }
        public DateOnly birthdayDate { get; set; }
        public Gender? gender { get; set; }
        public SkinColor? skinColor { get; set; }
        public bool? isPcd { get; set; }
    }
}