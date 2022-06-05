using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data {
    public class CurrencyConverterDbContext : DbContext {
        public CurrencyConverterDbContext(DbContextOptions<CurrencyConverterDbContext> options) : base(options) { }
    }
}
