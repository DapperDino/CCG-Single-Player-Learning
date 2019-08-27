using TheLiquidFire.AspectContainer;

public class PlayerIdleState : BaseState
{
    public override void Enter()
    {
        Temp_AutoChangeTurnForAI();
    }

    void Temp_AutoChangeTurnForAI()
    {
        if (container.GetMatch().CurrentPlayer.mode != ControlModes.Local)
        {
            container.GetAspect<MatchSystem>().ChangeTurn();
        }
    }
}