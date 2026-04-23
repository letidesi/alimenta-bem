using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlimentaBem.Src.Modules.User.Repository;
using User = AlimentaBem.Src.Modules.User.Repository.User;

namespace AlimentaBem.DataMappings;

public class UserMap : IEntityTypeConfiguration<User>
{
       public void Configure(EntityTypeBuilder<User> builder)
       {
              builder.HasMany(u => u.roles)
                     .WithOne(r => r.user)
                     .HasForeignKey(r => r.userId);
              builder.HasKey(u => u.id);
              builder.Property(u => u.name)
                     .HasColumnType("varchar(100)");
              builder.Property(u => u.email)
                    .HasColumnType("varchar(100)");
              builder.Property(u => u.name)
                     .HasColumnType("varchar(100)");
              builder.Property(u => u.passwordHash)
                     .HasColumnType("varchar(max)");
       }
}