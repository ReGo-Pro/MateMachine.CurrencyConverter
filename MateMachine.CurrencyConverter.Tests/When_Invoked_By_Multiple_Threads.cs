using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_Invoked_By_Multiple_Threads : ConverterTestBase {
        private double? result_usd_usd;
        private double? result_usd_cad;
        private double? result_cad_usd;
        private double? result_usd_gbp;
        private double? result_gbp_usd;
        private double? result_usd_sek;

        public async Task Act() {
            result_usd_usd = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "USD"), Currencies.Single(c => c.Name == "USD"), InitialAmount);
            result_usd_cad = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "USD"), Currencies.Single(c => c.Name == "CAD"), InitialAmount);
            result_cad_usd = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "CAD"), Currencies.Single(c => c.Name == "USD"), InitialAmount);
            result_usd_gbp = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "USD"), Currencies.Single(c => c.Name == "GBP"), InitialAmount);
            result_gbp_usd = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "GBP"), Currencies.Single(c => c.Name == "USD"), InitialAmount);
            result_usd_sek = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "USD"), Currencies.Single(c => c.Name == "SEK"), InitialAmount);
        }

        [TestMethod]
        public async Task Then_Just_One_Null() {
            await Act();
            Assert.IsNotNull(result_usd_usd);
            Assert.IsNotNull(result_usd_cad);
            Assert.IsNotNull(result_cad_usd);
            Assert.IsNotNull(result_usd_gbp);
            Assert.IsNotNull(result_gbp_usd);

            Assert.IsNull(result_usd_sek);
        }

        [TestMethod]
        public async Task Then_Results_Are_Verified() {
            await Act();
            Assert.AreEqual(10, Math.Round(result_usd_usd.Value, 5));
            Assert.AreEqual(13.4, Math.Round(result_usd_cad.Value, 5));
            Assert.AreEqual(7.46269, Math.Round(result_cad_usd.Value, 5));
            Assert.AreEqual(7.772, Math.Round(result_usd_gbp.Value, 5));
            Assert.AreEqual(12.86670, Math.Round(result_gbp_usd.Value, 5));
            Assert.AreEqual(null, result_usd_sek);
        }
    }
}
