using CCG.Containers;
using UnityEngine;

namespace CCG.Aspects
{
    public class GameViewSystem : MonoBehaviour, IAspect
    {
        private IContainer container = null;
        private ActionSystem actionSystem = null;

        public IContainer Container
        {
            get
            {
                if (container == null)
                {
                    container = GameFactory.Create();
                    container.AddAspect(this);
                }
                return container;
            }
            set
            {
                container = value;
            }
        }

        private void Awake()
        {
            container.Awake();
            actionSystem.Container.GetAspect<ActionSystem>();
        }

        private void Start()
        {
            Temp_SetupSinglePlayer();
            container.ChangeState<PlayerIdleState>();
        }

        private void Update()
        {
            actionSystem.Update();
        }

        private void Temp_SetupSinglePlayer()
        {
            Match match = container.GetMatch();
            match.Players[0].Mode = ControlModes.Local;
            match.Players[1].Mode = ControlModes.Computer;
        }
    }
}