using CCG.Cards;
using System.Collections.Generic;

namespace CCG
{
    public class Player
    {
        private const int MaxDeck = 30;
        private const int MaxHand = 10;
        private const int MaxBattlefield = 7;
        private const int MaxSecrets = 5;

        private readonly int index = 0;
        private Mana mana = new Mana();
        private int fatigue;

        private List<Card> hero = new List<Card>(1);
        private List<Card> weapon = new List<Card>(1);
        private List<Card> deck = new List<Card>(MaxDeck);
        private List<Card> hand = new List<Card>(MaxHand);
        private List<Card> battlefield = new List<Card>(MaxBattlefield);
        private List<Card> secrets = new List<Card>(MaxSecrets);
        private List<Card> graveyard = new List<Card>(MaxDeck);

        public Player(int index)
        {
            this.index = index;
        }

        public ControlModes Mode { get; set; } = ControlModes.Computer;

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
    }
}