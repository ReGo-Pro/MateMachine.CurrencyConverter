using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_Convert_Self : ConverterTestBase {
        [TestMethod]
        public async Task Then_Result_NotNull() {
            await Act("EUR", "EUR");
            Assert.IsNotNull(Result);
        }

        [TestMethod]
        public async Task Then_Result_Verified() {
            await Act("EUR", "EUR");
            Assert.AreEqual(10, Result);
        }
    }
}
