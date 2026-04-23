using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlimentaBem.Src.Modules.Role.Repository;
using Role = AlimentaBem.Src.Modules.Role.Repository.Role;

namespace AlimentaBem.DataMappings;

public class RoleMap : IEntityTypeConfiguration<Role>
{
       public void Configure(EntityTypeBuilder<Role> builder)
       {
              builder.HasOne(r => r.user)
                     .WithMany(r => r.roles)
                     .HasForeignKey(r => r.userId);
              builder.HasKey(r => r.id);
              builder.Property(r => r.type)
                     .HasColumnType("varchar(100)")
                     .HasConversion<string>()
                     .IsRequired(true);
       }
}