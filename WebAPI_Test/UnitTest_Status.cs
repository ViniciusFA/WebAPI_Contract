using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace WebAPI_UnitTest_Status
{
    [TestClass]
    public class UnitTest_Status
    {

        ContractController _contractController;

        [TestInitialize]
        public void TestInitialize()
        {
            _contractController = new ContractController();
        }

        [TestMethod]
        public void TestStatus_Baixado()
        {
            DateTime PayDate = DateTime.Now;

            Task<string> result = _contractController.GetStatusAsync(null, null, PayDate);
            Assert.IsTrue(result.Result == "Baixado");
        }

        [TestMethod]
        public void TestStatus_Aberta()
        {
            DateTime Duedate = DateTime.Now;
            DateTime CurrentyDate = DateTime.Now;

            Task<string> result = _contractController.GetStatusAsync(Duedate, CurrentyDate, null);
            Assert.IsTrue(result.Result == "Aberta");
        }

        [TestMethod]
        public void TestStatus_Atrasada()
        {
            DateTime Duedate = new DateTime(2021, 02, 02);
            DateTime CurrentyDate = DateTime.Now;

            Task<string> result = _contractController.GetStatusAsync(Duedate, CurrentyDate, null);
            Assert.IsTrue(result.Result == "Atrasada");
        }

    }
}
