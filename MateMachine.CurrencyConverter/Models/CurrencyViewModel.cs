using System.ComponentModel.DataAnnotations;

namespace MateMachine.CurrencyConverter.Models {
    public class CurrencyViewModel {
        [Required]
        [MaxLength(3)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string FullName { get; set; }
    }
}
