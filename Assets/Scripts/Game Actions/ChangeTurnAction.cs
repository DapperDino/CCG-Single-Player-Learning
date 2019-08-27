public class ChangeTurnAction : GameAction
{
    public int targetPlayerIndex;

    public ChangeTurnAction(int targetPlayerIndex)
    {
        this.targetPlayerIndex = targetPlayerIndex;
    }
}