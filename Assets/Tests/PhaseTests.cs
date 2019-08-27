using NUnit.Framework;
using System.Collections;
using TheLiquidFire.AspectContainer;

namespace Tests
{
    public class PhaseTests
    {
        private class TestAction : GameAction
        {
            public readonly int loopCount;
            public readonly int keyframe;
            public int Step { get; private set; }
            public bool IsComplete { get; private set; }

            public TestAction(int loopCount, int keyFrame)
            {
                this.loopCount = loopCount;
                keyframe = keyFrame;
            }

            public IEnumerator KeyFrameViewer(IContainer container, GameAction action)
            {
                for (Step = 0; Step < loopCount; ++Step)
                {
                    if (Step == keyframe)
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
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler);
            phase.viewer = action.KeyFrameViewer;

            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }
            Assert.AreEqual(action.Step, action.loopCount);
        }

        [Test]
        public void PhaseTriggersHandler()
        {
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler);
            phase.viewer = action.KeyFrameViewer;

            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }
            Assert.IsTrue(action.IsComplete);
        }

        [Test]
        public void PhaseTriggersHandlerWithoutKeyFrame()
        {
            var action = new TestAction(10, -1);
            var phase = new Phase(action, action.KeyFrameHandler);
            phase.viewer = action.KeyFrameViewer;

            var flow = phase.Flow(null);
            while (flow.MoveNext()) { }
            Assert.IsTrue(action.IsComplete);
        }

        [Test]
        public void PhaseTriggersOnKeyFrame()
        {
            var action = new TestAction(10, 5);
            var phase = new Phase(action, action.KeyFrameHandler);
            phase.viewer = action.KeyFrameViewer;

            var flow = phase.Flow(null);
            while (flow.MoveNext())
            {
                if (action.Step < action.keyframe)
                {
                    Assert.IsFalse(action.IsComplete);
                }
                else if (action.Step > action.keyframe)
                {
                    Assert.IsTrue(action.IsComplete);
                }
            }
        }
    }
}
