namespace AlimentaBem.Src.Modules.Donation.UseCases.Create.DTO;

public class DonationCreateRequest
{
    public Guid naturalPersonId { get; set; }
    public Guid organizationId { get; set; }
    public string itemName { get; set; }
    public int amountDonated { get; set; }
}