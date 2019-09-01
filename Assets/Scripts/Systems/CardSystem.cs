using System.Collections.Generic;
using TheLiquidFire.AspectContainer;

public class CardSystem : Aspect
{
    public List<Card> playable = new List<Card>();

    public void ChangeZone(Card card, Zones zone, Player toPlayer = null)
    {
        var fromPlayer = container.GetMatch().players[card.ownerIndex];
        toPlayer = toPlayer ?? fromPlayer;
        fromPlayer[card.zone].Remove(card);
        toPlayer[zone].Add(card);
        card.zone = zone;
        card.ownerIndex = toPlayer.index;
    }

    public void Refresh()
    {
        var match = container.GetMatch();
        playable.Clear();
        foreach (Card card in match.CurrentPlayer[Zones.Hand])
        {
            var playAction = new PlayCardAction(card);
            if (playAction.Validate())
                playable.Add(card);
        }
    }
}
