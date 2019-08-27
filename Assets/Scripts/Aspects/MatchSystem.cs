using CCG.Containers;
using CCG.GameActions;
using CCG.Notifications;

namespace CCG.Aspects
{
    public class MatchSystem : Aspect, IObserver
    {
        public void Awake() => this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);
        public void Destroy() => this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), Container);

        public void ChangeTurn()
        {
            Match match = Container.GetMatch();
            int nextIndex = (1 - match.CurrentPlayerIndex);
            ChangeTurn(nextIndex);
        }

        public void ChangeTurn(int index)
        {
            var action = new ChangeTurnAction(index);
            Container.Perform(action);
        }

        private void OnPerformChangeTurn(object sender, object args)
        {
            var action = args as ChangeTurnAction;
            Match match = Container.GetMatch();
            match.CurrentPlayerIndex = action.TargetPlayerIndex;
        }
    }
}
