using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

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

            var a = TimeSpan.Zero;
            Assert.IsTrue("THREE" == list[2]);
        }

    }
}
