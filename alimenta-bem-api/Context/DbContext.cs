using System.Linq.Expressions;
using AlimentaBem.EntityMetadata;
using AlimentaBem.Src.Modules.Donation.Repository;
using AlimentaBem.Src.Modules.Role.Repository;
using AlimentaBem.Src.Modules.User.Repository;
using AlimentaBem.Src.Modules.NaturalPerson.Repository;
using AlimentaBem.Src.Modules.Organization.Repository;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository;
using AlimentaBem.Src.Modules.UserOrganization.Repository;
using AlimentaBem.Src.Modules.PasswordReset.Repository;

namespace AlimentaBem.Context;

public class AlimentaBemContext : DbContext
{

    public AlimentaBemContext(DbContextOptions options) : base(options) { }

    #region
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<NaturalPerson> NaturalPersons { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationRequirement> OrganizationRequirements { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<UserOrganization> UserOrganizations { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    #endregion
    protected override void OnModelCreating(ModelBuilder mb)
    {
        ConfigureSoftDelete(ref mb);

        base.OnModelCreating(mb);
    }
    private void ConfigureSoftDelete(ref ModelBuilder mb)
    {
        foreach (var entityType in mb.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType);

                var body = Expression.Equal(

                    Expression.Property(parameter, nameof(BaseEntity.deletedAt)),

                    Expression.Constant(null, typeof(DateTime?))
                );

                mb.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }
        }
    }
}