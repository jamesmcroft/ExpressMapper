namespace ExpressMapper.Tests
{
    using System;
    using System.Collections.Generic;

    using ExpressMapper.Tests.Common;
    using ExpressMapper.Tests.Models.Classes;
    using ExpressMapper.Tests.Models.ViewModels;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionTests : BaseTestClass
    {
        [TestMethod]
        public void RegisteringCollectionTypesTest()
        {
            var exceptionMessage = string.Format(
                "It is invalid to register mapping for collection types from {0} to {1}, please use just class registration mapping and your collections will be implicitly processed. In case you want to include some custom collection mapping please use: Mapper.RegisterCustom.",
                typeof(List<Size>).FullName,
                typeof(SizeViewModel[]).FullName);
            Assert.ThrowsException<InvalidOperationException>(
                () => { Mapper.Register<List<Size>, SizeViewModel[]>(); },
                exceptionMessage);
        }
    }
}