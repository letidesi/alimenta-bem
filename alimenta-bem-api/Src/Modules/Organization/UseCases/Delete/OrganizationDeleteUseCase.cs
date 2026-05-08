using AlimentaBem.Context;
using AlimentaBem.Helpers;
using AlimentaBem.Src.Modules.Organization.Repository;

namespace AlimentaBem.Src.Modules.Organization.UseCases.Delete;

public class OrganizationDeleteUseCase
{
    private readonly AlimentaBemContext _context;
    private readonly Localizer _localizer;
    private readonly IOrganizationData _organizationData;

    public OrganizationDeleteUseCase(AlimentaBemContext context, Localizer localizer)
    {
        _context = context;
        _localizer = localizer;
        _organizationData = new OrganizationData(context);
    }

    public async Task exec(Guid id)
    {
        var existing = await _organizationData.ReadOne(id);

        if (existing is null)
            throw new Exception(_localizer["organization:NotFound"]);

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Remove vínculos de usuários com esta instituição
            var links = _context.UserOrganizations.Where(uo => uo.organizationId == id);
            _context.UserOrganizations.RemoveRange(links);

            await _organizationData.SoftDelete(existing);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
