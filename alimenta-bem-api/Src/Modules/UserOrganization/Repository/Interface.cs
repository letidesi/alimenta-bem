namespace AlimentaBem.Src.Modules.UserOrganization.Repository;

public interface IUserOrganizationData
{
    Task<UserOrganization> Create(UserOrganization userOrganization);
    Task LinkUserToOrganizations(Guid userId, IEnumerable<Guid> organizationIds);
    Task<List<Guid>> GetOrganizationIdsByUser(Guid userId);
    Task<bool> UserBelongsToOrganization(Guid userId, Guid organizationId);
    Task<bool> LinkExists(Guid userId, Guid organizationId);
    Task<bool> NaturalPersonBelongsToAdminOrgs(Guid adminUserId, Guid naturalPersonId);
    Task<bool> DonorUserBelongsToAdminOrgs(Guid adminUserId, Guid donorUserId);
    Task<bool> Unlink(Guid userId, Guid organizationId);
}
