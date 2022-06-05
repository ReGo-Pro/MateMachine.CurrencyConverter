using MateMachine.CurrencyConverter.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MateMachine.CurrencyConverter.Data.EntityConfigurations {
    internal class CurrencyEntityConfiguration : IEntityTypeConfiguration<Currency> {
        public void Configure(EntityTypeBuilder<Currency> builder) {
            builder.ToTable("Currencies");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(e => e.FullName)
                .HasMaxLength(128);

            builder.HasIndex(e => e.Name);
        }
    }
}
