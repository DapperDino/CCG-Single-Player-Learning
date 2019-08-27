using System.Collections.Generic;

public class Match
{
    public const int PlayerCount = 2;

    public List<Player> players = new List<Player>(PlayerCount);
    public int currentPlayerIndex;

    public Player CurrentPlayer
    {
        get
        {
            return players[currentPlayerIndex];
        }
    }

    public Player OpponentPlayer
    {
        get
        {
            return players[1 - currentPlayerIndex];
        }
    }

    public Match()
    {
        for (int i = 0; i < PlayerCount; ++i)
        {
            players.Add(new Player(i));
        }
    }
}