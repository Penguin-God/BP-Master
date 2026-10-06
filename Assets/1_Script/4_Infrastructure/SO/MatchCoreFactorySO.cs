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

    public MatchCore CreateMatchCore(BanPickStorage storage, ChampionCatalog championCatalog, Dictionary<Team, int> playerDatas, IPhaseEntry blueEntry, IPhaseEntry redEntry)
    {
        var dataProvider = playerDataProviderFactorySO.CreatePlayerDataProvider();
        var phaseAdvancer = gamePhaseLoder.CreateAdvacer();
        var phaseEventDispatcher = new PhaseEventDispatcher();
        var registry = masteryFactorySO.CreateRegistry(GetBoard(Team.Blue), GetBoard(Team.Red));
        var teamBonusCalculator = bonusDataFactory.CreateTeamBonusCalculator();

        var banPickHandler = new BanPickHandler(championCatalog, storage);
        banPickHandler.BanPickEventDispatcher.OnTeamChampionPick += (champion, team) =>
        {
            new MasteryApplier(registry.GetTeamMasteryCollection(team))
                .ApplyMastery(champion.Id, champion.Status);
        };

        var actionEventDispatcher = new BanPickEventDispatcher();
        var skillRunner = new SkillRunner(
            new SkillActionFactory(actionEventDispatcher, phaseEventDispatcher),
            new SkillCondtionFactory()
        );
        var skillController = new SkillUsecase(banPickHandler.PickSlotFacade.ChampionSlots, skillRunner);

        var teamDispatcher = new TeamPhaseEntryDispatcher(blueEntry, redEntry);
        var phaseManager = new PhaseFlowOrchestrator(phaseAdvancer, phaseEventDispatcher, teamDispatcher);

        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        banPickHandler.BanPickEventDispatcher.OnTeamBan += (team, _) => phaseManager.SubmitAction(team);

        var matchCore = new MatchCore(
            registry,
            phaseEventDispatcher,
            phaseAdvancer,
            banPickHandler,
            skillController,
            phaseManager,
            teamBonusCalculator
        );

        phaseEventDispatcher.OnPhaseDone += () =>
        {
            var result = MatchResultCalculator.CalculateResult(
                teamBonusCalculator,
                banPickHandler.PickSlotFacade.StatusSlots
            );
            matchCore.FinishGame(result);
        };

        return matchCore;

        MasteryBoardCollection GetBoard(Team team) => dataProvider.GetPlayer(playerDatas[team]).MasteryBoardCollection;
    }
}