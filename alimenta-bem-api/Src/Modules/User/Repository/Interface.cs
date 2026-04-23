namespace AlimentaBem.Src.Modules.User.Repository;

public interface IUserData
{
    Task<User> Create(User user);
    Task<User> Update(User user);
    Task<List<User>> ReadList();
    Task<User?> UpdateRole(Guid userId, string roleType);
    Task<User?> ReadOneByEmail(string email);
    Task<User?> ReadOne(Guid id);
}