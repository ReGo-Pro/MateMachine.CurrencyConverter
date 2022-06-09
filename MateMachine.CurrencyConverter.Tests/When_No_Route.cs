using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Tests {
    [TestClass]
    public class When_No_Route : ConverterTestBase {
        [TestMethod]
        public async Task Then_Result_IsNull() {
            await Act("USD", "SEK");
            Assert.IsNull(Result);
        }
    }
}
