namespace CCG.GameActions
{
    public class ChangeTurnAction : GameAction
    {
        public ChangeTurnAction(int targetPlayerIndex)
        {
            TargetPlayerIndex = targetPlayerIndex;
        }

        public int TargetPlayerIndex { get; set; } = 0;
    }
}
