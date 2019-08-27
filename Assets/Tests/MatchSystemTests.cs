using CCG.Aspects;
using CCG.Containers;
using CCG.GameActions;
using CCG.Notifications;
using NUnit.Framework;

namespace Tests
{
    public class MatchSystemTests
    {
        IContainer game;
        ActionSystem actionSystem;
        DataSystem dataSystem;
        MatchSystem matchSystem;
        TestSkipSystem testSkipSystem;

        private class TestSkipSystem : Aspect, IObserver
        {
            public bool enabled;

            public void Awake()
            {
                this.AddObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), Container);
            }

            public void Destroy()
            {
                this.RemoveObserver(OnPrepareChangeTurn, Global.PrepareNotification<ChangeTurnAction>(), Container);
            }

            private void OnPrepareChangeTurn(object sender, object args)
            {
                if (!enabled)
                    return;

                var action = args as ChangeTurnAction;
                action.TargetPlayerIndex = Container.GetMatch().CurrentPlayerIndex;
            }
        }

        [SetUp]
        public void TestSetup()
        {
            game = new Container();
            actionSystem = game.AddAspect<ActionSystem>();
            dataSystem = game.AddAspect<DataSystem>();
            matchSystem = game.AddAspect<MatchSystem>();
            testSkipSystem = game.AddAspect<TestSkipSystem>();
            game.Awake();
        }

        [TearDown]
        public void TestTearDown()
        {
            game.Destroy();
        }

        [Test]
        public void ChangingTurn_AddsAction()
        {
            // ASSERT
            Assert.IsFalse(actionSystem.IsActive);
            matchSystem.ChangeTurn(1);
            Assert.IsTrue(actionSystem.IsActive);
        }

        [Test]
        public void ChangingTurn_AppliesAction()
        {
            // ARRANGE
            dataSystem.Match.CurrentPlayerIndex = 0;
            int targetIndex = 1;

            // ACT
            matchSystem.ChangeTurn(targetIndex);
            RunToCompletion();

            // ASSERT
            Assert.AreEqual(dataSystem.Match.CurrentPlayerIndex, targetIndex);
        }

        [Test]
        public void DefaultChangeTurn_IsOpponentTurn()
        {
            for (int i = 0; i < 2; ++i)
            {
                int lastIndex = dataSystem.Match.CurrentPlayerIndex;
                matchSystem.ChangeTurn();
                RunToCompletion();
                Assert.AreNotEqual(dataSystem.Match.CurrentPlayerIndex, lastIndex);
            }
        }

        [Test]
        public void ModifyingChangeTurn_IsModified()
        {
            // ARRANGE
            testSkipSystem.enabled = true;
            int startIndex = dataSystem.Match.CurrentPlayerIndex;

            // ACT
            matchSystem.ChangeTurn();
            RunToCompletion();

            // ASSERT
            Assert.AreEqual(dataSystem.Match.CurrentPlayerIndex, startIndex);
        }

        void RunToCompletion()
        {
            while (actionSystem.IsActive)
            {
                actionSystem.Update();
            }
        }
    }
}
