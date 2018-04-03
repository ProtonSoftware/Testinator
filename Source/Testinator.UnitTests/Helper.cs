using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Testinator.UnitTests
{
    [TestClass]
    public class Helper
    {
        [TestMethod]
        public void X()
        {
            var list = new List<string>() { "one", "two", "three", "four"};
            var index = list.IndexOf("three");
            list.Remove("three");
            list.Insert(index, "THREE");
            Assert.IsTrue("THREE" == list[2]);
        }

    }
}
