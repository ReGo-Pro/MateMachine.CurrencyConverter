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
            neighbours.AddRange(edges.Where(e => e.FromCurrency.Name == node.Name).Select(e => e.ToCurrency));
            neighbours.AddRange(edges.Where(e => e.ToCurrency.Name == node.Name).Select(e => e.FromCurrency));
            return neighbours;
        }
    }
}
