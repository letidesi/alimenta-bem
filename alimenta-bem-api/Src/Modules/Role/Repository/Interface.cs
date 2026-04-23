namespace AlimentaBem.Src.Modules.Role.Repository;

public interface IRoleData
{
    Task<Role> Create(Role role);
}