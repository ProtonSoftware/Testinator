using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testinator.UnitTests
{
    /// <summary>
    /// Tutorial https://www.youtube.com/watch?v=HYrXogLj7vg
    /// </summary>
    [TestClass]
    public class NameOfTestedClassTest
    {
        [TestMethod]
        public void NameOfFuntionToTest_Scenario_ExpectedBehaviour()
        {
            // Arrange
                                 // Make an object of a class we are testing
            // Act
                                 // Do something with this object and catch the result
            // Assert
            Assert.IsTrue(true); // Replace "true" with the result from "act" part, it shall check if test is passed
        }
    }
}
