namespace CCG
{
    public class Match
    {
        private const int PlayerCount = 2;

        public Match()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                Players[i] = new Player(i);
            }
        }

        public Player[] Players { get; } = new Player[PlayerCount];
        public int CurrentPlayerIndex { get; set; } = 0;
        public Player CurrentPlayer => Players[CurrentPlayerIndex];
        public Player OtherPlayer => Players[1 - CurrentPlayerIndex];
    }
}
