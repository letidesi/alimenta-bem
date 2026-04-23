namespace AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;

public class DonationCreateResponse
{
    public Guid id { get; set; }
    public Guid naturalPersonId { get; set; }
    public Guid organizationId { get; set; }
    public string itemName { get; set; }
    public int amountDonated { get; set; }
    public string status { get; set; }

    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset? updatedAt { get; set; }
}