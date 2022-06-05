using Microsoft.EntityFrameworkCore;
using MateMachine.CurrencyConverter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MateMachine.CurrencyConverter.Data.EntityConfigurations {
    internal class CurrencyExchangeRateEntityConfiguration : IEntityTypeConfiguration<CurrenyExchangeRate> {
        public void Configure(EntityTypeBuilder<CurrenyExchangeRate> builder) {
            builder.ToTable("ExchangeRates");
        }
    }
}
