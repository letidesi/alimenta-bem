namespace AlimentaBem.Src.Modules.Donation.UseCases.ReadListByOrganization.DTO;

public class DonationReadListByOrganizationResponse
{
    public List<DonationByOrganizationItem> donations { get; set; } = new();
}

public class DonationByOrganizationItem
{
    public Guid id { get; set; }
    public Guid naturalPersonId { get; set; }
    public string donorName { get; set; }
    public string itemName { get; set; }
    public int amountDonated { get; set; }
    public string status { get; set; }
    public string? unavailableMessage { get; set; }
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset? reviewedAt { get; set; }
    public DateTimeOffset? receivedAt { get; set; }
}
