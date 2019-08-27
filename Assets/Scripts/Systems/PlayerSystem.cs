using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using TheLiquidFire.Extensions;

public class PlayerSystem : Aspect, IObserve
{

    public void Awake()
    {
        this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
        this.AddObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
        this.RemoveObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), container);
    }

    void OnPerformChangeTurn(object sender, object args)
    {
        var action = args as ChangeTurnAction;
        var match = container.GetAspect<DataSystem>().match;
        var player = match.players[action.targetPlayerIndex];
        DrawCards(player, 1);
    }

    void OnPerformDrawCards(object sender, object args)
    {
        var action = args as DrawCardsAction;
        action.cards = action.player[Zones.Deck].Draw(action.amount);
        action.player[Zones.Hand].AddRange(action.cards);
    }

    void DrawCards(Player player, int amount)
    {
        var action = new DrawCardsAction(player, amount);
        container.AddReaction(action);
    }
}