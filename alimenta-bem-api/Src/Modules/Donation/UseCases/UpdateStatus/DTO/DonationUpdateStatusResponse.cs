namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;

public class DonationUpdateStatusResponse
{
    public Guid id { get; set; }
    public Guid naturalPersonId { get; set; }
    public Guid organizationId { get; set; }
    public string itemName { get; set; }
    public int amountDonated { get; set; }
    public string status { get; set; }
    public string? unavailableReason { get; set; }
    public string? unavailableMessage { get; set; }
    public DateTimeOffset? reviewedAt { get; set; }
    public DateTimeOffset? receivedAt { get; set; }
    public DateTimeOffset? updatedAt { get; set; }
}
