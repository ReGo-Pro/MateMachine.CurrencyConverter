using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_Convert_Direct : ConverterTestBase {
        [TestMethod]
        public async Task Then_Result_NotNull() {
            await Act("USD", "CAD");
            Assert.IsNotNull(Result);
        }

        [TestMethod]
        public async Task Then_Result_Verified() {
            await Act("USD", "CAD");
            Assert.AreEqual(13.4, Result);
        }
    }
}
