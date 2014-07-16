using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bocami.Practices.WebApi.Tests
{
    [TestClass]
    public class HttpConfigurationExtensionsTests
    {
        [TestMethod]
        public void RegisterCompositeHttpControllerTypeResolverTest()
        {
            var config = new HttpConfiguration();
            config.RegisterCompositeHttpControllerTypeResolver();

            var expected = typeof(CompositeHttpControllerTypeResolver);
            var actual = config.Services.GetHttpControllerTypeResolver().GetType();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RegisterCompositeHttpControllerSelectorTest()
        {
            var config = new HttpConfiguration();
            config.RegisterCompositeHttpControllerSelector();

            var expected = typeof(CompositeHttpControllerSelector);
            var actual = config.Services.GetHttpControllerSelector().GetType();

            Assert.AreEqual(expected, actual);
        }
    }
}
