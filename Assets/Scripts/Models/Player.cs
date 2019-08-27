using System.Collections.Generic;

public class Player
{
    public const int maxDeck = 30;
    public const int maxHand = 10;
    public const int maxBattlefield = 7;
    public const int maxSecrets = 5;

    public readonly int index;
    public ControlModes mode;
    public Mana mana = new Mana();
    public int fatigue;

    public List<Card> hero = new List<Card>(1);
    public List<Card> weapon = new List<Card>(1);
    public List<Card> deck = new List<Card>(maxDeck);
    public List<Card> hand = new List<Card>(maxHand);
    public List<Card> battlefield = new List<Card>(maxBattlefield);
    public List<Card> secrets = new List<Card>(maxSecrets);
    public List<Card> graveyard = new List<Card>(maxDeck);

    public List<Card> this[Zones z]
    {
        get
        {
            switch (z)
            {
                case Zones.Hero:
                    return hero;
                case Zones.Weapon:
                    return weapon;
                case Zones.Deck:
                    return deck;
                case Zones.Hand:
                    return hand;
                case Zones.Battlefield:
                    return battlefield;
                case Zones.Secrets:
                    return secrets;
                case Zones.Graveyard:
                    return graveyard;
                default:
                    return null;
            }
        }
    }

    public Player(int index)
    {
        this.index = index;
    }
}