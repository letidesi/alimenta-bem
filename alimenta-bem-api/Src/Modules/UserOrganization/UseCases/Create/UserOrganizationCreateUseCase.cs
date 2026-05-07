using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.UserOrganization.UseCases.Create.DTO;
using AlimentaBem.Src.Modules.UserOrganization.Repository;

namespace AlimentaBem.Src.Modules.UserOrganization.UseCases.Create;

using UserOrganizationEntity = AlimentaBem.Src.Modules.UserOrganization.Repository.UserOrganization;

public class UserOrganizationCreateUseCase
{
    private readonly IUserOrganizationData _data;
    private readonly Localizer _localizer;

    public UserOrganizationCreateUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _data = new UserOrganizationData(context);
        _localizer = localizer;
    }

    public async Task<UserOrganizationEntity> exec(Guid requestingAdminId, UserOrganizationCreateRequest req)
    {
        if (!await _data.UserBelongsToOrganization(requestingAdminId, req.organizationId))
            throw new Exception(_localizer["userOrganization:NoAccess"]);

        if (await _data.LinkExists(req.userId, req.organizationId))
            throw new Exception(_localizer["userOrganization:AlreadyLinked"]);

        return await _data.Create(new UserOrganizationEntity
        {
            userId = req.userId,
            organizationId = req.organizationId
        });
    }
}
