public class Card
{
    public string id;
    public string name;
    public string text;
    public int cost;
    public int orderOfPlay = int.MaxValue;
    public int ownerIndex;
    public Zones zone = Zones.Deck;
}