using CCG;
using CCG.Aspects;
using CCG.Containers;
using NUnit.Framework;

namespace Tests
{
    public class ContainerTests
    {
        private class TestAspect : IAspect { public IContainer Container { get; set; } }
        private class AltTestAspect : IAspect { public IContainer Container { get; set; } }

        [Test]
        public void AddingAspectToContainer_AspectIsAdded()
        {
            // ARRANGE
            var container = new Container();

            // ACT
            container.AddAspect<TestAspect>();

            // ASSERT
            Assert.AreEqual(container.Aspects.Count, 1);
        }

        [Test]
        public void AddingMultipleAspects_AllAspectsAreAdded()
        {
            // ARRANGE
            var container = new Container();

            // ACT
            container.AddAspect<TestAspect>();
            container.AddAspect<AltTestAspect>();

            // ASSERT
            Assert.AreEqual(container.Aspects.Count, 2);
        }

        [Test]
        public void RequestingAspectWithNoKey_GetsAspect()
        {
            // ARRANGE
            var container = new Container();

            // ACT
            var original = container.AddAspect<TestAspect>();
            var fetch = container.GetAspect<TestAspect>();

            // ASSERT
            Assert.AreSame(original, fetch);
        }

        [Test]
        public void RequestingAspectWithKey_GetsAspect()
        {
            // ARRANGE
            var container = new Container();

            // ACT
            var original = container.AddAspect<TestAspect>("Foo");
            var fetch = container.GetAspect<TestAspect>("Foo");

            // ASSERT
            Assert.AreSame(original, fetch);
        }

        [Test]
        public void RequestingMissingAspect_ReturnsNull()
        {
            // ARRANGE
            var container = new Container();

            // ACT
            var fetch = container.GetAspect<TestAspect>();

            // ASSERT
            Assert.IsNull(fetch);
        }

        [Test]
        public void AddingPreCreatedAspect_AspectIsAdded()
        {
            // ARRANGE
            var container = new Container();
            var aspect = new TestAspect();

            // ACT
            container.AddAspect(aspect);

            // ASSERT
            Assert.IsNotEmpty(container.Aspects);
        }

        [Test]
        public void RequestingPreCreatedAspect_GetsAspect()
        {
            // ARRANGE
            var container = new Container();
            var original = new TestAspect();

            // ACT
            container.AddAspect(original);
            var fetch = container.GetAspect<TestAspect>();

            // ASSERT
            Assert.AreSame(original, fetch);
        }

        [Test]
        public void AddingAspectToContainer_AspectKnowsTheirContainer()
        {
            // ARRANGE
            var container = new Container();

            // ACT
            var aspect = container.AddAspect<TestAspect>();

            // ASSERT
            Assert.IsNotNull(aspect.Container);
        }
    }
}
