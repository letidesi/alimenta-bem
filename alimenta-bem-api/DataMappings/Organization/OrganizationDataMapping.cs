using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlimentaBem.Src.Modules.Organization.Repository;
using Organization = AlimentaBem.Src.Modules.Organization.Repository.Organization;

namespace AlimentaBem.DataMappings;

public class OrganizationMap : IEntityTypeConfiguration<Organization>
{
       public void Configure(EntityTypeBuilder<Organization> builder)
       {
              builder.HasKey(o => o.id);
              builder.Property(o => o.name)
                     .HasColumnType("varchar(150)");
              builder.Property(o => o.type)
                     .HasColumnType("varchar(40)")
                     .HasConversion<string>();
              builder.Property(o => o.description)
                     .HasColumnType("varchar(500)");
       }
}