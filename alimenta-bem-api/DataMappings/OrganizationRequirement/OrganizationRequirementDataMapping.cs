using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlimentaBem.Src.Modules.Organization.Repository;
using AlimentaBem.Src.Modules.OrganizationRequirement.Repository;
using OrganizationRequirement = AlimentaBem.Src.Modules.OrganizationRequirement.Repository.OrganizationRequirement;

namespace AlimentaBem.DataMappings;

public class OrganizationRequirementMap : IEntityTypeConfiguration<OrganizationRequirement>
{
       public void Configure(EntityTypeBuilder<OrganizationRequirement> builder)
       {
              builder.HasOne(or => or.organization)
                     .WithMany(o => o.OrganizationRequirements)
                     .HasForeignKey(or => or.id);
              builder.HasKey(or => or.id);
              builder.Property(or => or.itemName)
                     .HasColumnType("varchar(150)");
              builder.Property(or => or.quantity);
              builder.Property(or => or.type)
                     .HasColumnType("varchar(40)")
                     .HasConversion<string>()
;
       }
}