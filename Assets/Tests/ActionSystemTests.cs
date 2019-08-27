using CCG.Aspects;
using CCG.Containers;
using CCG.GameActions;
using CCG.Notifications;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Tests
{
    public class ActionSystemTests
    {

        private class TestAction : GameAction
        {
            public bool didPrepare;
            public bool didPerform;
        }

        private class NotificationMarks
        {
            public bool sequenceBegin;
            public bool sequenceEnd;
            public bool complete;
            public bool prepare;
            public bool perform;
            public bool deathReaper;
        }

        private class TestSystem : Aspect
        {
            public const int rootActionOrder = 0;
            public const int depthCheckPriority = 1;
            public const int depthReactionOrder = int.MinValue;
            public NotificationMarks actionMarks = new NotificationMarks();
            public NotificationMarks reactionMarks = new NotificationMarks();
            public List<TestAction> reactions = new List<TestAction>();
            public bool loopedDeath;
            public bool depthFirst;

            public void Setup()
            {
                this.AddObserver(OnSequenceBegin, ActionSystem.BeginSequenceNotification);
                this.AddObserver(OnSequenceEnd, ActionSystem.EndSequenceNotification);
                this.AddObserver(OnComplete, ActionSystem.CompleteNotification);
                this.AddObserver(OnPrepare, Global.PrepareNotification<TestAction>());
                this.AddObserver(OnPerform, Global.PerformNotification<TestAction>());
                this.AddObserver(OnDeath, ActionSystem.DeathReaperNotification);
            }

            public void TearDown()
            {
                this.RemoveObserver(OnSequenceBegin, ActionSystem.BeginSequenceNotification);
                this.RemoveObserver(OnSequenceEnd, ActionSystem.EndSequenceNotification);
                this.RemoveObserver(OnComplete, ActionSystem.CompleteNotification);
                this.RemoveObserver(OnPrepare, Global.PrepareNotification<TestAction>());
                this.RemoveObserver(OnPerform, Global.PerformNotification<TestAction>());
                this.RemoveObserver(OnDeath, ActionSystem.DeathReaperNotification);
            }

            void OnSequenceBegin(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
                marks.sequenceBegin = true;

                action.Prepare.Viewer = TestViewer;
                action.Perform.Viewer = TestViewer;
            }

            void OnSequenceEnd(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
                marks.sequenceEnd = true;
            }

            void OnComplete(object sender, object args)
            {
                actionMarks.complete = true;
            }

            void OnPrepare(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
                marks.prepare = true;
                action.didPrepare = true;
            }

            void OnPerform(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
                marks.perform = true;
                action.didPerform = true;

                if (action.OrderOfPlay != rootActionOrder)
                {
                    reactions.Add(action);
                }
                else
                {
                    AddReactions((IContainer)sender);
                }
                if (action.Priority == depthCheckPriority)
                {
                    var reaction = new TestAction
                    {
                        OrderOfPlay = depthReactionOrder
                    };
                    ((IContainer)sender).GetAspect<ActionSystem>().AddReaction(reaction);
                }
                if (action.OrderOfPlay == depthReactionOrder)
                {
                    depthFirst = (reactions.Count == 2);
                }
            }

            void OnDeath(object sender, object args)
            {
                var action = args as TestAction;
                var marks = action.OrderOfPlay == rootActionOrder ? actionMarks : reactionMarks;

                if (actionMarks.deathReaper == false)
                {
                    var reaction = new TestAction
                    {
                        OrderOfPlay = int.MaxValue
                    };
                    ((ActionSystem)sender).AddReaction(reaction);
                }
                else
                {
                    loopedDeath = true;
                }

                marks.deathReaper = true;
            }

            IEnumerator TestViewer(IContainer game, GameAction action)
            {
                yield return null;
                yield return true;
                yield return null;
            }

            void AddReactions(IContainer game)
            {
                for (int i = 0; i < 5; ++i)
                {
                    var reaction = new TestAction
                    {
                        OrderOfPlay = UnityEngine.Random.Range(1, 100)
                    };
                    if (i == 2)
                        reaction.Priority = depthCheckPriority;
                    game.GetAspect<ActionSystem>().AddReaction(reaction);
                }
            }
        }

        IContainer game;
        ActionSystem actionSystem;
        TestSystem testSystem;

        [SetUp]
        public void TestSetup()
        {
            NotificationCenter.instance.Clean();
            game = new Container();
            actionSystem = game.AddAspect<ActionSystem>();
            testSystem = game.AddAspect<TestSystem>();
            testSystem.Setup();
        }

        [TearDown]
        public void TestTearDown()
        {
            testSystem.TearDown();
        }

        void RunToCompletion()
        {
            var timeOut = 0;
            while (actionSystem.IsActive && timeOut < 1000)
            {
                timeOut++;
                actionSystem.Update();
            }
        }

        [Test]
        public void testActionSystemTracksActiveState()
        {
            actionSystem.Perform(new TestAction());
            Assert.IsTrue(actionSystem.IsActive);
            RunToCompletion();
            Assert.IsFalse(actionSystem.IsActive);
        }

        [Test]
        public void testActionNotifications()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();
            var m = testSystem.actionMarks;
            var result = m.sequenceBegin &&
                         m.sequenceEnd &&
                         m.complete &&
                         m.prepare &&
                         m.perform &&
                         m.deathReaper;
            Assert.IsTrue(result);
        }

        [Test]
        public void testReactionNotifications()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();
            var m = testSystem.reactionMarks;
            var result = m.sequenceBegin &&
                m.sequenceEnd &&
                m.prepare &&
                m.perform &&
                !m.complete &&
                !m.deathReaper;
            Assert.IsTrue(result);
        }

        [Test]
        public void testReactionsAreSorted()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();
            int priority = int.MaxValue;
            int orderOfPlay = int.MinValue;
            for (int i = 0; i < testSystem.reactions.Count; ++i)
            {
                var reaction = testSystem.reactions[i];
                Assert.LessOrEqual(reaction.Priority, priority);
                if (reaction.Priority != priority)
                {
                    priority = reaction.Priority;
                    orderOfPlay = int.MinValue;
                }
                Assert.GreaterOrEqual(reaction.OrderOfPlay, orderOfPlay);
                orderOfPlay = reaction.OrderOfPlay;
            }
        }

        [Test]
        public void testCancelAction()
        {
            var action = new TestAction();
            action.Cancel();
            actionSystem.Perform(action);
            RunToCompletion();
            Assert.IsFalse(action.didPrepare);
            Assert.IsFalse(action.didPerform);
        }

        [Test]
        public void testLoopableDeathPhase()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();
            Assert.IsTrue(testSystem.loopedDeath);
        }

        [Test]
        public void testDepthFirstReactions()
        {
            actionSystem.Perform(new TestAction());
            RunToCompletion();
            Assert.IsTrue(testSystem.depthFirst);
        }

        [Test]
        public void testActionTypesHaveUniqueIDs()
        {
            var id1 = new GameAction().ID;
            var id2 = new TestAction().ID;
            Assert.AreNotEqual(id1, id2);
        }
    }
}
