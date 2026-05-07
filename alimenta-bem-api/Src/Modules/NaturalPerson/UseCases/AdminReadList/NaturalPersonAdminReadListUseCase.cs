using AlimentaBem.Context;
using AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminReadList.DTO;

namespace AlimentaBem.Src.Modules.NaturalPerson.UseCases.AdminReadList;

public class NaturalPersonAdminReadListUseCase
{
    private readonly AlimentaBemContext _context;

    public NaturalPersonAdminReadListUseCase(AlimentaBemContext context)
    {
        _context = context;
    }

    public async Task<NaturalPersonAdminReadListResponse> exec(IEnumerable<Guid> adminOrganizationIds)
    {
        var orgIds = adminOrganizationIds.ToList();

        var naturalPersons = await _context.NaturalPersons
            .Where(n => _context.Donations.Any(d => d.naturalPersonId == n.id && orgIds.Contains(d.organizationId)))
            .Select(n => new NaturalPersonAdminReadListItem
            {
                id = n.id,
                userId = n.userId,
                name = n.name,
                socialName = n.socialName,
                emailUser = n.emailUser,
                age = n.age,
                birthdayDate = n.birthdayDate,
                gender = n.gender.HasValue ? n.gender.Value.ToString() : null,
                skinColor = n.skinColor.HasValue ? n.skinColor.Value.ToString() : null,
                isPcd = n.isPcd,
                donationCount = _context.Donations.Count(d => d.naturalPersonId == n.id && orgIds.Contains(d.organizationId))
            })
            .OrderBy(n => n.name)
            .ToListAsync();

        return new NaturalPersonAdminReadListResponse
        {
            naturalPersons = naturalPersons
        };
    }
}
