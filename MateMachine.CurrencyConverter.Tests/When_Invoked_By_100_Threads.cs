using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_Invoked_By_100_Threads : ConverterTestBase {
        double?[] resultArray = new double?[100];

        public async Task Act() {
            for (int i = 0; i < 100; i++) {
                resultArray[i] = await Task.Run(() => CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == "USD"), Currencies.Single(c => c.Name == "GBP"), InitialAmount));
            }
        }

        [TestMethod]
        public async Task Then_Results_Are_Valid() {
            await Act();
            for (int i = 0; i < 100; i++) {
                Assert.AreEqual(7.772, Math.Round(resultArray[i].Value, 5));
            }
        }
    }
}
