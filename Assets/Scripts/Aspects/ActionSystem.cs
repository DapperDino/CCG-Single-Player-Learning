using CCG.Containers;
using CCG.GameActions;
using CCG.Notifications;
using System.Collections;
using System.Collections.Generic;

namespace CCG.Aspects
{
    public class ActionSystem : Aspect
    {
        public static string BeginSequenceNotification => "ActionSystem.beginSequenceNotification";
        public static string EndSequenceNotification => "ActionSystem.endSequenceNotification";
        public static string DeathReaperNotification => "ActionSystem.deathReaperNotification";
        public static string CompleteNotification => "ActionSystem.completeNotification";

        private GameAction rootAction = null;
        private IEnumerator rootSequence = null;
        private List<GameAction> openReactions = null;

        public bool IsActive => rootSequence != null;

        public void Perform(GameAction action)
        {
            if (IsActive) { return; }
            rootAction = action;
            rootSequence = Sequence(action);
        }

        public void Update()
        {
            if (rootSequence == null) { return; }
            rootAction = null;
            rootSequence = null;
            openReactions = null;
            this.PostNotification(CompleteNotification);
        }

        public void AddReaction(GameAction action)
        {
            if (openReactions == null) { return; }
            openReactions.Add(action);
        }

        private IEnumerator Sequence(GameAction action)
        {
            this.PostNotification(BeginSequenceNotification, action);

            var phase = MainPhase(action.Prepare);
            while (phase.MoveNext()) { yield return null; }

            phase = MainPhase(action.Perform);
            while (phase.MoveNext()) { yield return null; }

            if (rootAction == action)
            {
                phase = EventPhase(DeathReaperNotification, action, true);
                while (phase.MoveNext()) { yield return null; }
            }

            this.PostNotification(EndSequenceNotification, action);
        }

        private IEnumerator MainPhase(Phase phase)
        {
            if (phase.Owner.IsCanceled) { yield break; }

            var reactions = openReactions = new List<GameAction>();
            IEnumerator flow = phase.Flow(Container);
            while (flow.MoveNext()) { yield return null; }

            flow = ReactPhase(reactions);
            while (flow.MoveNext()) { yield return null; }
        }

        private IEnumerator ReactPhase(List<GameAction> reactions)
        {
            reactions.Sort(SortActions);
            foreach (GameAction reaction in reactions)
            {
                IEnumerator subFlow = Sequence(reaction);

                while (subFlow.MoveNext()) { yield return null; }
            }
        }

        private IEnumerator EventPhase(string notification, GameAction action, bool repeats = false)
        {
            List<GameAction> reactions = null;
            do
            {
                reactions = openReactions = new List<GameAction>();
                this.PostNotification(notification, action);

                var phase = ReactPhase(reactions);
                while (phase.MoveNext()) { yield return null; }
            } while (repeats == true && reactions.Count > 0);
        }

        private int SortActions(GameAction a, GameAction b)
        {
            if (a.Priority != b.Priority) { return b.Priority.CompareTo(a.Priority); }
            else { return a.OrderOfPlay.CompareTo(b.OrderOfPlay); }
        }
    }

    public static class ActionSystemExtensions
    {
        public static void Perform(this IContainer game, GameAction action)
        {
            var actionSystem = game.GetAspect<ActionSystem>();
            actionSystem.Perform(action);
        }

        public static void AddReaction(this IContainer game, GameAction action)
        {
            var actionSystem = game.GetAspect<ActionSystem>();
            actionSystem.AddReaction(action);
        }
    }
}
