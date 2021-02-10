using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebAPI.Controllers;

namespace WebAPI_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestStatus_Baixado()
        {
            DateTime PayDate = DateTime.Now;

            string result = GetStatus(null, null, PayDate);
            Assert.IsTrue(result == "Baixado");
        }

        [TestMethod]
        public void TestStatus_Aberta()
        {
            DateTime Duedate = DateTime.Now;
            DateTime CurrentyDate = DateTime.Now;

            string result = GetStatus(Duedate, CurrentyDate, null);
            Assert.IsTrue(result == "Aberta");
        }

        [TestMethod]
        public void TestStatus_Atrasada()
        {
            DateTime Duedate = new DateTime(2021, 02, 02);
            DateTime CurrentyDate = DateTime.Now;

            string result = GetStatus(Duedate, CurrentyDate, null);
            Assert.IsTrue(result == "Atrasada");
        }

        public string GetStatus(DateTime? DueDate, DateTime? CurrentyDate, DateTime? PayDate)
        {
            if (PayDate == null)
            {
                if (DueDate >= CurrentyDate)
                    return "Aberta";
                else
                    return "Atrasada";
            }
            return "Baixado";
        }

    }
}
