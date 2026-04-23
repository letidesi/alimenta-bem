using AlimentaBem.EntityMetadata;

namespace AlimentaBem.Helpers;

public class UserSummary : BaseEntity
{
    public string name { get; set; }
    public string email { get; set; }
}