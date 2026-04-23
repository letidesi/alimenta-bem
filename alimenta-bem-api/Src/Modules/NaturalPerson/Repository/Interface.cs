namespace AlimentaBem.Src.Modules.NaturalPerson.Repository
{
    using User = AlimentaBem.Src.Modules.User.Repository.User;

    public interface INaturalPersonData
    {
        Task<User?> GetUserByEmail(string email);
        Task<NaturalPerson> Create(NaturalPerson naturalPerson);
        Task<NaturalPerson> Update(NaturalPerson naturalPerson);
        Task<List<NaturalPerson>> ReadList();
        Task<NaturalPerson> ReadNaturalPersonByUser(Guid userId);
        Task<NaturalPerson> CheckNaturalPersonAlreadyExist(NaturalPerson naturalPerson);
        Task<NaturalPerson?> ReadOneByUserId(Guid userId);
        Task<NaturalPerson?> ReadOneById(Guid id);
        Task SoftDelete(NaturalPerson naturalPerson);
    }
}