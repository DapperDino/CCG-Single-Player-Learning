namespace CCG.Cards
{
    public class Card
    {
        private string id = string.Empty;
        private string name = "New Card Name";
        private string text = "New Card Text";
        private int cost = 0;
        private int orderOfPlay = int.MaxValue;
        private int ownerIndex = 0;
        private Zones zone = Zones.Deck;
    }
}
