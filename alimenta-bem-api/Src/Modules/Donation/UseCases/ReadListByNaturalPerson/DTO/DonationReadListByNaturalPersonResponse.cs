namespace AlimentaBem.Src.Modules.Donation.UseCases.ReadListByNaturalPerson.DTO;

public class DonationReadListByNaturalPersonResponse
{
    public List<DonationByNaturalPersonItem> donations { get; set; } = new();
}

public class DonationByNaturalPersonItem
{
    public Guid id { get; set; }
    public Guid organizationId { get; set; }
    public string organizationName { get; set; }
    public string itemName { get; set; }
    public int amountDonated { get; set; }
    public string status { get; set; }
    public string? unavailableMessage { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset? reviewedAt { get; set; }
    public DateTimeOffset? receivedAt { get; set; }
}
