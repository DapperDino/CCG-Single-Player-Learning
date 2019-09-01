using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class DeathSystem : Aspect, IObserve
{
    public void Awake()
    {
        this.AddObserver(OnDeathReaperNotification, ActionSystem.deathReaperNotification);
        this.AddObserver(OnPerformDeath, Global.PerformNotification<DeathAction>(), container);
    }
    public void Destroy()
    {
        this.RemoveObserver(OnDeathReaperNotification, ActionSystem.deathReaperNotification);
        this.RemoveObserver(OnPerformDeath, Global.PerformNotification<DeathAction>(), container);
    }

    void OnDeathReaperNotification(object sender, object args)
    {
        var match = container.GetMatch();
        foreach (Player player in match.players)
        {
            foreach (Card card in player[Zones.Battlefield])
            {
                if (ShouldReap(card))
                    TriggerReap(card);
            }
        }
    }

    void OnPerformDeath(object sender, object args)
    {
        var action = args as DeathAction;
        var cardSystem = container.GetAspect<CardSystem>();
        cardSystem.ChangeZone(action.card, Zones.Graveyard);
    }

    bool ShouldReap(Card card)
    {
        return card is IDestructable target && target.hitPoints <= 0;
    }

    void TriggerReap(Card card)
    {
        var action = new DeathAction(card);
        container.AddReaction(action);
    }
}