using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataProviderFactorySO", menuName = "Data/PlayerDataProviderFactorySO")]
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
