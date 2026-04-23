namespace AlimentaBem.Src.Modules.Donation.UseCases.UpdateStatus.DTO;

public class DonationUpdateStatusRequest
{
    public Guid donationId { get; set; }
    public Guid organizationId { get; set; }
    public string status { get; set; }
    public string? unavailableReason { get; set; }
}
