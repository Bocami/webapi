using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bocami.Practices.WebApi.Tests
{
    public abstract class Command
    {
    }

    public class CommandController<TCommand> where TCommand : Command
    {
    }

    public class TestCommand : Command
    {
    }

    [TestClass]
    public class FirstGenericTypeArgumentAsControllerNameHttpControllerSelectorTests
    {
        [TestMethod]
        public void GetControllerNameTest()
        {
            var controllerType = typeof (CommandController<TestCommand>);

            var expected = "Test";

            var actual = FirstGenericTypeArgumentAsControllerNameHttpControllerSelector.GetControllerName(controllerType);

            Assert.AreEqual(expected, actual);

        }
    }
}
