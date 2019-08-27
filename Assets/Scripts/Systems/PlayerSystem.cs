using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using TheLiquidFire.Extensions;
using UnityEngine;

public class PlayerSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
        this.AddObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), container);
        this.AddObserver(OnPerformFatigue, Global.PerformNotification<FatigueAction>(), container);
        this.AddObserver(OnPerformOverDraw, Global.PerformNotification<OverdrawAction>(), container);
    }

    public void Destroy()
    {
        this.RemoveObserver(OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction>(), container);
        this.RemoveObserver(OnPerformDrawCards, Global.PerformNotification<DrawCardsAction>(), container);
        this.RemoveObserver(OnPerformFatigue, Global.PerformNotification<FatigueAction>(), container);
        this.RemoveObserver(OnPerformOverDraw, Global.PerformNotification<OverdrawAction>(), container);
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

        int deckCount = action.player[Zones.Deck].Count;
        int fatigueCount = Mathf.Max(action.amount - deckCount, 0);
        for (int i = 0; i < fatigueCount; ++i)
        {
            var fatigueAction = new FatigueAction(action.player);
            container.AddReaction(fatigueAction);
        }

        int roomInHand = Player.maxHand - action.player[Zones.Hand].Count;
        int overDraw = Mathf.Max((action.amount - fatigueCount) - roomInHand, 0);
        if (overDraw > 0)
        {
            var overDrawAction = new OverdrawAction(action.player, overDraw);
            container.AddReaction(overDrawAction);
        }

        int drawCount = action.amount - fatigueCount - overDraw;
        action.cards = action.player[Zones.Deck].Draw(drawCount);
        action.player[Zones.Hand].AddRange(action.cards);
    }

    void OnPerformFatigue(object sender, object args)
    {
        var action = args as FatigueAction;
        action.player.fatigue++;
    }

    void OnPerformOverDraw(object sender, object args)
    {
        var action = args as OverdrawAction;
        action.cards = action.player[Zones.Deck].Draw(action.amount);
        action.player[Zones.Graveyard].AddRange(action.cards);
    }

    void DrawCards(Player player, int amount)
    {
        var action = new DrawCardsAction(player, amount);
        container.AddReaction(action);
    }
}