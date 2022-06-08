using MateMachine.CurrencyConverter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Business {
    internal class CurrencyConversionGraph : Graph<Currency, CurrencyExchangeRate> {
        protected override IEnumerable<Currency> GetNeighbours(Currency node) {
            var neighbours = new List<Currency>();
            neighbours.AddRange(edges.Where(e => e.FromCurrency == node).Select(e => e.ToCurrency));
            neighbours.AddRange(edges.Where(e => e.ToCurrency == node).Select(e => e.FromCurrency));
            return neighbours;
        }
    }
}
