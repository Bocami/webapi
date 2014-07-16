using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bocami.Practices.WebApi.Tests
{
    public class ATestController : ApiController
    {
    }

    public class BTestController : ApiController
    {
    }

    public class FakeHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        private readonly ICollection<Type> controllerTypes;

        public FakeHttpControllerTypeResolver(params Type[] controllerTypes)
        {
            this.controllerTypes = controllerTypes.ToList();
        }

        public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            return controllerTypes;
        }
    }

    [TestClass]
    public class CompositeHttpControllerTypeResolverTests
    {
        [TestMethod]
        public void GetControllerTypesDistinctTest()
        {
            var httpControllerTypeResolver = new CompositeHttpControllerTypeResolver(
                new FakeHttpControllerTypeResolver(typeof(ATestController)),
                new FakeHttpControllerTypeResolver(typeof(ATestController))
            );

            var expected = 1;
            var actual = httpControllerTypeResolver.GetControllerTypes(null).Count();

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void GetControllerTypesTest()
        {
            var httpControllerTypeResolver = new CompositeHttpControllerTypeResolver(
                new FakeHttpControllerTypeResolver(typeof(ATestController)),
                new FakeHttpControllerTypeResolver(typeof(BTestController))
            );

            var expected = 2;
            var actual = httpControllerTypeResolver.GetControllerTypes(null).Count();

            Assert.AreEqual(expected, actual);

        }
    }
}
