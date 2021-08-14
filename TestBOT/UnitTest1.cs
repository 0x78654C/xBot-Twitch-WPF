using Microsoft.VisualStudio.TestTools.UnitTesting;
using xBot_WPF;

namespace TestBOT
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            test t = new test();
            int c = t.testSum(1, 2);
            Assert.AreEqual(c, 3);
        }
    }
}
