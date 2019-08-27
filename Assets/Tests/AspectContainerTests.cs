using NUnit.Framework;
using TheLiquidFire.AspectContainer;

namespace Tests
{
    public class AspectContainerTests
    {
        private class TestAspect : IAspect
        {
            public IContainer container { get; set; }
        }

        private class AltTestAspect : IAspect
        {
            public IContainer container { get; set; }
        }

        [Test]
        public void TestContainerCanAddAspect()
        {
            var container = new Container();
            container.AddAspect<TestAspect>();
            Assert.AreEqual(container.Aspects().Count, 1);
        }

        [Test]
        public void TestContainerCanAddMultipleAspects()
        {
            var container = new Container();
            container.AddAspect<TestAspect>("Test1");
            container.AddAspect<TestAspect>("Test2");
            Assert.AreEqual(container.Aspects().Count, 2);
        }

        [Test]
        public void TestContainerCanAddMultipleTypesOfAspects()
        {
            var container = new Container();
            container.AddAspect<TestAspect>();
            container.AddAspect<AltTestAspect>();
            Assert.AreEqual(container.Aspects().Count, 2);
        }

        [Test]
        public void TestContainerCanGetAspectWithNoKey()
        {
            var container = new Container();
            var original = container.AddAspect<TestAspect>();
            var fetch = container.GetAspect<TestAspect>();
            Assert.AreSame(original, fetch);
        }

        [Test]
        public void TestContainerCanGetAspectWithKey()
        {
            var container = new Container();
            var original = container.AddAspect<TestAspect>("Foo");
            var fetch = container.GetAspect<TestAspect>("Foo");
            Assert.AreSame(original, fetch);
        }

        [Test]
        public void TestContainerCanTryGetMissingAspect()
        {
            var container = new Container();
            var fetch = container.GetAspect<TestAspect>("Foo");
            Assert.IsNull(fetch);
        }

        [Test]
        public void TestContainerCanAddPreCreatedAspect()
        {
            var container = new Container();
            var aspect = new TestAspect();
            container.AddAspect(aspect);
            Assert.IsNotEmpty(container.Aspects());
        }

        [Test]
        public void TestContainerCanGetPreCreatedAspect()
        {
            var container = new Container();
            var original = new TestAspect();
            container.AddAspect(original);
            var fetch = container.GetAspect<TestAspect>();
            Assert.AreSame(original, fetch);
        }

        [Test]
        public void TestAspectTracksItsContainer()
        {
            var container = new Container();
            var aspect = container.AddAspect<TestAspect>();
            Assert.IsNotNull(aspect.container);
        }
    }
}
