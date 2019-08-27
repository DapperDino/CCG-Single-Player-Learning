using TheLiquidFire.AspectContainer;

public class CardSystem : Aspect
{
    public void ChangeZone(Card card, Zones zone, Player toPlayer = null)
    {
        var fromPlayer = container.GetMatch().players[card.ownerIndex];
        toPlayer = toPlayer ?? fromPlayer;
        fromPlayer[card.zone].Remove(card);
        toPlayer[zone].Add(card);
        card.zone = zone;
        card.ownerIndex = toPlayer.index;
    }
}
