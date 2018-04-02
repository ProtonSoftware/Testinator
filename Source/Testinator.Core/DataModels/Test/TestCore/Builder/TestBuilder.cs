namespace Testinator.Core
{
    public class TestBuilder : IBuilder<Test>
    {
        #region Private Members

        private Test mTest = new Test();

        public Test GetResult() => mTest;

        #endregion
    }
}
