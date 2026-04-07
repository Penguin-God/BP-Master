using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MatchCoreFactory", menuName = "BP Master/MatchCoreFactory")]
public class MatchCoreFactorySO : ScriptableObject
{
    [SerializeField] GamePhaseLoderSO gamePhaseLoder;
    [SerializeField] MasteryRegistryFactorySO masteryFactorySO;
    [SerializeField] PlayerDataProviderFactorySO playerDataProviderFactorySO;
    [SerializeField] TeamBonusDataSO bonusDataFactory;

    public MatchCore CreateMatchCore(BanPickStorage storage, ChampionCatalog championCatalog, Dictionary<Team, int> playerDatas)
    {
        var dataProvider = playerDataProviderFactorySO.CreatePlayerDataProvider();

        var phaseAdvancer = gamePhaseLoder.CreateAdvacer();
        var registry = masteryFactorySO.CreateRegistry(GetBoard(Team.Blue), GetBoard(Team.Red));

        return new MatchCore(championCatalog, storage, phaseAdvancer, registry, bonusDataFactory.CreateTeamBonusCalculator());

        MasteryBoardCollection GetBoard(Team team) => dataProvider.GetPlayer(playerDatas[team]).MasteryBoardCollection;
    }
}