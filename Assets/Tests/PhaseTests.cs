using CCG;
using CCG.Containers;
using CCG.GameActions;
using NUnit.Framework;
using System.Collections;

namespace Tests
{
    public class PhaseTests
    {

        private class TestAction : GameAction
        {
            public int LoopCount { get; }
            public int Keyframe { get; }
            public int Step { get; private set; }
            public bool IsComplete { get; private set; }

            public TestAction(int loopCount, int keyFrame)
            {
                LoopCount = loopCount;
                Keyframe = keyFrame;
            }

            public IEnumerator KeyFrameViewer(IContainer container, GameAction action)
            {
                for (Step = 0; Step < LoopCount; ++Step)
                {
                    if (Step == LoopCount)
                    {
                        yield return true;
                    }
                    else
                    {
                        yield return false;
                    }
                }
            }

            public void KeyFrameHandler(IContainer container)
            {
                IsComplete = true;
            }
        }

        [Test]
        public void PhaseRunsCompleteViewerFlow()
        {
            // ARRANGE
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler)
            {
                Viewer = action.KeyFrameViewer
            };
            
            // ACT
            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }

            // ASSERT
            Assert.AreEqual(action.Step, action.LoopCount);
        }

        [Test]
        public void PhaseTriggersHandler()
        {
            // ARRANGE
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler)
            {
                Viewer = action.KeyFrameViewer
            };

            // ACT
            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }

            // ASSERT
            Assert.IsTrue(action.IsComplete);
        }

        [Test]
        public void PhaseTriggersHandlerWithoutKeyFrame()
        {
            // ARRANGE
            var action = new TestAction(10, -1);
            var phase = new Phase(action, action.KeyFrameHandler)
            {
                Viewer = action.KeyFrameViewer
            };

            // ACT
            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }

            // ASSERT
            Assert.IsTrue(action.IsComplete);
        }

        [Test]
        public void PhaseTriggersOnKeyFrame()
        {
            // ARRANGE
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler)
            {
                Viewer = action.KeyFrameViewer
            };
            var flow = phase.Flow(null);

            // ACT
            while (flow.MoveNext())
            {
                // ASSERT
                if (action.Step < action.Keyframe)
                {
                    Assert.IsFalse(action.IsComplete);
                }
                else if (action.Step > action.Keyframe)
                {
                    Assert.IsTrue(action.IsComplete);
                }
            }
        }
    }
}
