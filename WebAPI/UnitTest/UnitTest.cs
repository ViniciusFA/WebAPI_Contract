using NUnit.Framework;
using WebAPI.Controllers;

namespace WebAPI.UnitTest
{
    [TestClass]
    public class UnitTest   
    {
        [TestMethod]
        public void TestStatus_Baixado(){
            
            DateTime PayDate = DateTime.Now;            

            string result = Controllers.GetStatus(null, null, PayDate);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestStatus_Aberta(){

            //data venciemnto >= data atual e data pagamnto vazia
            DateTime Duedate = DateTime.Now;
            DateTime CurrentyDate = DateTime.Now;            

            string result = Controllers.GetStatus(Duedate, CurrentyDate, null);
            Assert.IsTrue(result);
        }

         [TestMethod]
        public void TestStatus_Atrasada(){

            //data venciemnto < data atual e data pagamnto vazia
            DateTime Duedate = new DateTime(2021, 02, 02);
            DateTime CurrentyDate = DateTime.Now;            

            string result = Controllers.GetStatus(Duedate, CurrentyDate, null);
            Assert.IsTrue(result);
        }
    }
}