using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AlimentaBem.Src.Modules.PasswordReset.Repository;

namespace AlimentaBem.DataMappings;

public class PasswordResetMap : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.HasKey(p => p.id);
        builder.Property(p => p.token)
               .HasColumnType("varchar(64)")
               .IsRequired();
        builder.HasIndex(p => p.token);
        builder.HasIndex(p => new { p.userId, p.used });
    }
}
