using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_Convert_Indirect : ConverterTestBase {
        [TestMethod]
        public async Task Then_Result_NotNull() {
            await Act("EUR", "USD");
            Assert.IsNotNull(Result);
        }

        [TestMethod]
        public async Task Then_Result_Verified() {
            await Act("EUR", "USD");
            Assert.AreEqual(11.62791, Result);
        }
    }
}
