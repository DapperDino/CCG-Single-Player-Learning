using CCG.Containers;
using CCG.GameActions;
using System;
using System.Collections;

namespace CCG
{
    public class Phase
    {
        private readonly Action<IContainer> handler = delegate { };

        public Phase(GameAction owner, Action<IContainer> handler)
        {
            Owner = owner;
            this.handler = handler;
        }

        public GameAction Owner { get; } = null;
        public Func<IContainer, GameAction, IEnumerator> Viewer { get; set; } = null;

        public IEnumerator Flow(IContainer game)
        {
            var hitKeyFrame = false;

            if (Viewer != null)
            {
                IEnumerator sequence = Viewer.Invoke(game, Owner);
                while (sequence.MoveNext())
                {
                    var isKeyFrame = (sequence.Current is bool) ? (bool)sequence.Current : false;
                    if (isKeyFrame)
                    {
                        hitKeyFrame = true;
                        handler.Invoke(game);
                    }
                    yield return null;
                }
            }

            if (!hitKeyFrame)
            {
                handler.Invoke(game);
            }
        }
    }
}
