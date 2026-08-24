using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataProviderFactorySO", menuName = "Factory/PlayerDataProviderFactorySO")]
public class PlayerDataProviderFactorySO : ScriptableObject
{
    [SerializeField] string mainPlayerName = "@@";
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] AIPlayerDataCatalogSO aiPlayerDataCatalog;

    public PlayerDataProvider CreatePlayerDataProvider()
    {
        IPlayerDataLoader localLoader = new LocalPlayerDataLoader(mainPlayerName, new JsonMasterySaver());
        return new PlayerDataProvider(matchConfigSO.UserId, localLoader, aiPlayerDataCatalog);
    }
}

public class LocalPlayerDataLoader : IPlayerDataLoader
{
    readonly string playerName;
    readonly JsonMasterySaver saver;

    public LocalPlayerDataLoader(string playerName, JsonMasterySaver saver)
    {
        this.playerName = playerName;
        this.saver = saver;
    }

    public PlayerData LoadPlayer(int id)
    {
        var inventory = saver.Load();
        if (inventory == null)
            inventory = new MasteryProfile(0);
        return new PlayerData(id, playerName, inventory.BoardCollection);
    }
}