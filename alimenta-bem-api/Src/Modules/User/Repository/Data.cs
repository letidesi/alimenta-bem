using AlimentaBem.Helpers;
using AlimentaBem.Context;

namespace AlimentaBem.Src.Modules.User.Repository;

using Role = AlimentaBem.Src.Modules.Role.Repository.Role;

public class UserData : IUserData
{
    private readonly AlimentaBemContext _context;

    public UserData(AlimentaBemContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<User> Create(User user)
    {
        user = (User)WhiteSpaces.RemoveFromAllStringProperty(user);

        _context.Users.Add(user);

        _ = await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> Update(User user)
    {
        user = (User)WhiteSpaces.RemoveFromAllStringProperty(user);

        _context.Users.Update(user);

        _ = await _context.SaveChangesAsync();

        return user;
    }

    public async Task<List<User>> ReadList()
    {
        return await _context.Users
            .AsNoTracking()
            .Include(u => u.roles)
            .ToListAsync();
    }

    public async Task<User?> UpdateRole(Guid userId, string roleType)
    {
        var user = await _context.Users
            .Where(u => u.id.Equals(userId))
            .Where(u => u.deletedAt == null)
            .FirstOrDefaultAsync();

        if (user is null)
            return null;

        var currentRoles = await _context.Roles
            .Where(role => role.userId == user.id)
            .ToListAsync();

        if (currentRoles.Any())
            _context.Roles.RemoveRange(currentRoles);

        _context.Roles.Add(new Role
        {
            userId = user.id,
            type = roleType
        });

        _ = await _context.SaveChangesAsync();

        return await _context.Users
            .AsNoTracking()
            .Include(u => u.roles)
            .Where(u => u.id.Equals(userId))
            .Where(u => u.deletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> ReadOneByEmail(string email)
    {
        var user = await _context.Users
            .Include(u => u.roles)
            .Where(u => u.email.Equals(email))
            .Where(u => u.deletedAt == null)
            .FirstOrDefaultAsync();

        return user;
    }
    public async Task<User?> ReadOne(Guid id)
    {
        var user = await _context.Users
            .Include(u => u.roles)
            .Where(u => u.id.Equals(id))
            .Where(u => u.deletedAt == null)
            .FirstOrDefaultAsync();

        return user;
    }
}