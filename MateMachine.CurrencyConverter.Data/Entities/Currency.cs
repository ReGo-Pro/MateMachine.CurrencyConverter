using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data.Entities {
    public class Currency : IEquatable<Currency> {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }

        public bool Equals(Currency? other) {
            if (other == null) return false;
            if (this.Name.ToUpper() == other.Name.ToUpper()) return true;
            return false;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Currency);
        }

        public override int GetHashCode() {
            int hash = 13;
            hash = (hash * 7) + Id.GetHashCode();
            hash = (hash * 7) + FullName.GetHashCode();
            hash = (hash * 7) + Name.GetHashCode();
            return hash;
        }
    }
}
