using AlimentaBem.Context;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Helpers;

public static class AdminOrganizationGuard
{
    public static async Task EnsureAccess(AlimentaBemContext context, Guid adminUserId, Guid organizationId, Localizer localizer)
    {
        var data = new UserOrganizationData(context);
        var hasAccess = await data.UserBelongsToOrganization(adminUserId, organizationId);
        if (!hasAccess)
            throw new Exception(localizer["userOrganization:NoAccess"]);
    }

    public static async Task EnsureDonorUserAccess(AlimentaBemContext context, Guid adminUserId, Guid donorUserId, Localizer localizer)
    {
        var data = new UserOrganizationData(context);
        var hasAccess = await data.DonorUserBelongsToAdminOrgs(adminUserId, donorUserId);
        if (!hasAccess)
            throw new Exception(localizer["naturalPerson:NoAccess"]);
    }
}
