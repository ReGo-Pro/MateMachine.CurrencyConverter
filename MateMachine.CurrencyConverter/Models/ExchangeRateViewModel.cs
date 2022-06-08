using System.ComponentModel.DataAnnotations;

namespace MateMachine.CurrencyConverter.Models {
    public class ExchangeRateViewModel {
        [Required]
        [MaxLength(3)]
        public string FromCurrency { get; set; }

        [Required]
        [MaxLength(3)]
        public string ToCurrency { get; set; }

        [Required]
        public double ExchangeRate { get; set; }
    }
}
