using System.Collections.Generic;

public class DrawCardsAction : GameAction
{
    public int amount;
    public List<Card> cards;

    public DrawCardsAction(Player player, int amount)
    {
        this.player = player;
        this.amount = amount;
    }
}