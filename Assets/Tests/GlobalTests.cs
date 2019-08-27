using NUnit.Framework;

namespace Tests
{
    public class GlobalTests
    {
        private class Foo { }
        private class Bar { }

        [Test]
        public void RequestGenericAndTypeIDs_IDsAreEqual()
        {
            // ARRANGE
            var id1 = Global.GenerateID<Foo>();
            var type = new Foo().GetType();
            var id2 = Global.GenerateID(type);

            // ASSERT
            Assert.AreEqual(id1, id2);
        }

        [Test]
        public void RequestUniqueTypeIDs_IDsAreNotEqual()
        {
            // ARRANGE
            var id1 = Global.GenerateID<Foo>();
            var id2 = Global.GenerateID<Bar>();

            // ASSERT
            Assert.AreNotEqual(id1, id2);
        }

        [Test]
        public void RequestGenericAndTypePrepareNotifications_NotificationsAreEqual()
        {
            // ARRANGE
            var prepare1 = Global.PrepareNotification<Foo>();
            var foo = new Foo();
            var prepare2 = Global.PrepareNotification(foo.GetType());

            // ASSERT
            Assert.AreEqual(prepare1, prepare2);
        }

        [Test]
        public void RequestGenericAndTypePerformNotifications_NotificationsAreEqual()
        {
            // ARRANGE
            var perform1 = Global.PerformNotification<Foo>();
            var foo = new Foo();
            var perform2 = Global.PerformNotification(foo.GetType());

            // ASSERT
            Assert.AreEqual(perform1, perform2);
        }

        [Test]
        public void RequestPrepareAndPerformNotifications_NotificationsAreNotEqual()
        {
            // ARRANGE
            var prepare = Global.PrepareNotification<Foo>();
            var perform = Global.PerformNotification<Foo>();

            // ASSERT
            Assert.AreNotEqual(prepare, perform);
        }

        [Test]
        public void RequestDiffernetPrepareNotificationTypes_NotificationsAreNotEqual()
        {
            // ARRANGE
            var foo = Global.PrepareNotification<Foo>();
            var bar = Global.PrepareNotification<Bar>();

            // ASSERT
            Assert.AreNotEqual(foo, bar);
        }

        [Test]
        public void RequestDiffernetPerformNotificationTypes_NotificationsAreNotEqual()
        {
            // ARRANGE
            var foo = Global.PerformNotification<Foo>();
            var bar = Global.PerformNotification<Bar>();

            // ASSERT
            Assert.AreNotEqual(foo, bar);
        }
    }
}
