namespace CCG.Models
{
    public class Match
    {
        private readonly Player[] players = new Player[PlayerCount];
        private int currentPlayerIndex = 0;

        private const int PlayerCount = 2;

        public Match()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                players[i] = new Player(i);
            }
        }

        public Player CurrentPlayer => players[currentPlayerIndex];
        public Player OtherPlayer => players[1 - currentPlayerIndex];
    }
}
