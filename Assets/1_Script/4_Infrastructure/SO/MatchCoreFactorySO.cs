using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MatchCoreFactory", menuName = "SO/Match/MatchCoreFactory")]
public class MatchCoreFactorySO : ScriptableObject
{
    public int UserId = 1;
    [SerializeField] string mainPlayerName = "@@";

    [SerializeField] GamePhaseLoderSO gamePhaseLoder;
    [SerializeField] MasteryRegistryFactorySO masteryFactorySO;
    [SerializeField] AIPlayerDataCatalogSO aiPlayerDataCatalog;

    public MatchCore CreateMatchCore(BanPickStorage storage, ChampionCatalog championCatalog, Dictionary<Team, int> playerDatas)
    {
        IPlayerDataLoader localLoader = new LocalPlayerDataLoader(mainPlayerName, new JsonMasterySaver());
        var dataProvider = new PlayerDataProvider(UserId, localLoader, aiPlayerDataCatalog);

        var phaseAdvancer = gamePhaseLoder.CreateAdvacer();
        var registry = masteryFactorySO.CreateRegistry(GetBoard(Team.Blue), GetBoard(Team.Red));

        return new MatchCore(championCatalog, storage, phaseAdvancer, registry);

        MasteryBoardCollection GetBoard(Team team) => dataProvider.GetPlayer(playerDatas[team]).MasteryBoardCollection;
    }
}