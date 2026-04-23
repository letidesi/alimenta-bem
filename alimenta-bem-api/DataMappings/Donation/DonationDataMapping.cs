using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlimentaBem.Src.Modules.Donation.Repository;

namespace AlimentaBem.DataMappings;

public class DonationMap : IEntityTypeConfiguration<Donation>
{
       public void Configure(EntityTypeBuilder<Donation> builder)
       {
              builder.HasOne(d => d.naturalPerson)
                     .WithMany(d => d.donations)
                     .HasForeignKey(d => d.naturalPersonId)
                     .OnDelete(DeleteBehavior.Cascade);
              builder.HasOne(d => d.organization)
                     .WithMany(d => d.donations)
                     .HasForeignKey(d => d.organizationId)
                     .OnDelete(DeleteBehavior.Cascade);
              builder.HasKey(o => o.id);
              builder.Property(o => o.itemName)
                     .HasColumnType("varchar(150)");
              builder.Property(o => o.amountDonated);
       }
}