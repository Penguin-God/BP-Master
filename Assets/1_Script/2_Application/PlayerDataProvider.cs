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

    public PlayerData GetPlayer(int id)
    {
        if (id == mainPlayerId)
        {
            return localLoader.LoadPlayer(id);
        }

        return aiLoader.LoadPlayer(id);
    }
}