using System.Collections.Generic;

public class PlayerDataProvider
{
    readonly int mainPlayerId;
    readonly IPlayerDataLoader localLoader;
    readonly IPlayerDataLoader aiLoader;

    public PlayerDataProvider(int mainPlayerId, IPlayerDataLoader localLoader, IPlayerDataLoader aiLoader)
    {
        this.mainPlayerId = mainPlayerId;
        this.localLoader = localLoader;
        this.aiLoader = aiLoader;
    }

    public PlayerData GetPlayer(int id) => id == mainPlayerId ? localLoader.LoadPlayer(id) : aiLoader.LoadPlayer(id);

    public Dictionary<Team, PlayerData> GetTeamPlayersDict(int blueId, int redId) 
        => new Dictionary<Team, PlayerData>
            {
                { Team.Blue, GetPlayer(blueId) },
                { Team.Red, GetPlayer(redId) }
            };
}