using CCG.Containers;
using CCG.Notifications;

namespace CCG.GameActions
{
    public class GameAction
    {
        public int ID { get; } = 0;
        public Player Player { get; set; }
        public int Priority { get; set; }
        public int OrderOfPlay { get; set; }
        public bool IsCanceled { get; protected set; }
        public Phase Prepare { get; protected set; }
        public Phase Perform { get; protected set; }

        public GameAction()
        {
            ID = Global.GenerateID(GetType());
            Prepare = new Phase(this, OnPrepareKeyFrame);
            Perform = new Phase(this, OnPerformKeyFrame);
        }

        public virtual void Cancel() => IsCanceled = true;

        protected virtual void OnPrepareKeyFrame(IContainer game)
        {
            var notificationName = Global.PrepareNotification(GetType());
            game.PostNotification(notificationName, this);
        }

        protected virtual void OnPerformKeyFrame(IContainer game)
        {
            var notificationName = Global.PerformNotification(GetType());
            game.PostNotification(notificationName, this);
        }
    }
}