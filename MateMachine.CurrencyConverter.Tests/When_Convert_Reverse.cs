using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_Convert_Reverse : ConverterTestBase {
        [TestMethod]
        public async Task Then_Result_NotNull() {
            await Act("USD", "GBP");
            Assert.IsNotNull(Result);
        }

        [TestMethod]
        public async Task Then_Result_Verified() {
            await Act("USD", "GBP");
            Assert.AreEqual(7.772, Result);
        }

        [TestMethod]
        public async Task Then_Result_Reverse_Verified() {
            await Act("GBP", "USD");
            Assert.AreEqual(12.86670, Result);
        }
    }
}
