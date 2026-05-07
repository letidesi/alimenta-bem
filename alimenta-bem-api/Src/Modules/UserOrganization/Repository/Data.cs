using AlimentaBem.Context;
using Microsoft.EntityFrameworkCore;

namespace AlimentaBem.Src.Modules.UserOrganization.Repository;

public class UserOrganizationData : IUserOrganizationData
{
    private readonly AlimentaBemContext _context;

    public UserOrganizationData(AlimentaBemContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<UserOrganization> Create(UserOrganization userOrganization)
    {
        _context.UserOrganizations.Add(userOrganization);
        await _context.SaveChangesAsync();
        return userOrganization;
    }

    public async Task LinkUserToOrganizations(Guid userId, IEnumerable<Guid> organizationIds)
    {
        foreach (var orgId in organizationIds)
        {
            var exists = await LinkExists(userId, orgId);
            if (!exists)
                _context.UserOrganizations.Add(new UserOrganization { userId = userId, organizationId = orgId });
        }
        await _context.SaveChangesAsync();
    }

    public Task<List<Guid>> GetOrganizationIdsByUser(Guid userId)
    {
        return _context.UserOrganizations
            .Where(uo => uo.userId == userId)
            .Select(uo => uo.organizationId)
            .ToListAsync();
    }

    public Task<bool> UserBelongsToOrganization(Guid userId, Guid organizationId)
    {
        return _context.UserOrganizations
            .AnyAsync(uo => uo.userId == userId && uo.organizationId == organizationId);
    }

    public Task<bool> LinkExists(Guid userId, Guid organizationId)
    {
        return _context.UserOrganizations
            .AnyAsync(uo => uo.userId == userId && uo.organizationId == organizationId);
    }

    public Task<bool> NaturalPersonBelongsToAdminOrgs(Guid adminUserId, Guid naturalPersonId)
    {
        var adminOrgIds = _context.UserOrganizations
            .Where(uo => uo.userId == adminUserId)
            .Select(uo => uo.organizationId);

        return _context.Donations
            .AnyAsync(d => d.naturalPersonId == naturalPersonId && adminOrgIds.Contains(d.organizationId));
    }

    public Task<bool> DonorUserBelongsToAdminOrgs(Guid adminUserId, Guid donorUserId)
    {
        var adminOrgIds = _context.UserOrganizations
            .Where(uo => uo.userId == adminUserId)
            .Select(uo => uo.organizationId);

        var naturalPersonIds = _context.NaturalPersons
            .Where(n => n.userId == donorUserId)
            .Select(n => n.id);

        return _context.Donations
            .AnyAsync(d => naturalPersonIds.Contains(d.naturalPersonId) && adminOrgIds.Contains(d.organizationId));
    }
}
