namespace ExpressMapper.Tests.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class BaseTestClass
    {
        [TestCleanup]
        public void Cleanup()
        {
            Mapper.Reset();
        }
    }
}