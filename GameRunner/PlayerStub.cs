using System.Collections.Generic;

public class PlayerStub
{
    public static List<Player> CreatePlayers()
    {
        
        List<Player> players = new List<Player>
        {
            CreatePlayer("Alice"),
            CreatePlayer("Bob"),
            CreatePlayer("Chad"),
            CreatePlayer("Dani")
        };

        return players;
    }

    private static Player CreatePlayer(string name)
    {
        // Customize the player creation logic as needed
        return new Player(name);
    }
}