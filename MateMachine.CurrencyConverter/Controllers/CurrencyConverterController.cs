using MateMachine.CurrencyConverter.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MateMachine.CurrencyConverter.Controllers {
    [Route("api/CurrencyConverter")]
    public class CurrencyConverterController : Controller {
        private IUnitOfWork _uow;
        public CurrencyConverterController(IUnitOfWork uow) {
            _uow = uow;
        }

        [HttpGet("Currencies")]
        public IActionResult GetAllCurrencies() {
            return Ok(_uow.CurrencyRepo.GetAll());
        }
    }
}
