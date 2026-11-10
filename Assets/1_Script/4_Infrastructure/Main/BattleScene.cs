using Match;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] AI_Main ai_main;
    [SerializeField] ChampionSelector_UI championSelector;

    Dictionary<Team, int> playerIds = new();
    [SerializeField] MatchCoreFactorySO matchCoreFactorySO;
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] UI_SelectCheck uI_SelectCheck;

    int ai_id;
    BanPickStorage storage;
    public void GameStart(Team playerTeam)
    {
        ai_id = MatchContext.MatchState.GetOpponentId(matchConfigSO.UserId);
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerIds.Add(playerTeam, matchConfigSO.UserId);
        playerIds.Add(aiTeam, ai_id);

        storage = MatchContext.CreateFearlessStorage();
        var championCatalog = new ChampionCatalog(storage.SelectableIds.Select(x => ChampionDataLoder.GetChampionData(x).CreateChampion()));
        var (blue, red) = CreatePhaseOrchestrator(championSelector, ai_main, playerTeam);
        var banPickDispatcher = new BanPickEventDispatcher();
        var core = matchCoreFactorySO.CreateMatchCore(storage, new BanPickHandler(championCatalog, storage, banPickDispatcher), playerIds, blue, red);

        matchUI_Controller.Init(playerTeam, core, playerIds); // start보다 먼저

        ai_main.Init(ai_id, aiTeam, storage, core.SkillController, championCatalog, core.MasteryRegistry, core.BanPickHandler, core.PhaseAdvancer, core.PhaseManager);

        BattleTutorialEventBinder.BindBattleTutorial(uI_SelectCheck.CreateConditionTutorialAction, core.PhaseEventDispatcher, storage);
        if (MatchContext.MatchState.TotalWins > 0) uI_SelectCheck.CreateConditionTutorialAction(TutorialTriggerType.Fearless)();


        core.OnGameFinished += new BattleResultHandler(storage, playerIds, matchConfigSO, ai_id).OnDone;
        core.PhaseManager.Start();
    }

    (IPhaseEntry blue, IPhaseEntry red) CreatePhaseOrchestrator(IPhaseEntry player, IPhaseEntry ai, Team playerTeam)
    {
        IPhaseEntry blue = playerTeam == Team.Blue ? player : ai;
        IPhaseEntry red = playerTeam == Team.Red ? player : ai;
        return (blue, red);
    }
}

public class BattleResultHandler
{
    BanPickStorage storage;
    Dictionary<Team, int> playerIds;
    MatchConfigSO matchConfigSO;
    int ai_id;

    public BattleResultHandler(BanPickStorage storage, Dictionary<Team, int> playerIds, MatchConfigSO matchConfigSO, int ai_id)
    {
        this.storage = storage;
        this.playerIds = playerIds;
        this.matchConfigSO = matchConfigSO;
        this.ai_id = ai_id;
    }

    public void OnDone(MatchResult result)
    {
        int winnerId = BattleResultLogic.GetWinnerId(playerIds, result.Winner);
        bool matchEnd = MatchContext.RecordMatchResult(winnerId, storage.PickIds.GetAll());

        Object.FindAnyObjectByType<MatchResultView>(FindObjectsInactive.Include).DrawResult(result, CreateGameEndButtonModel(matchEnd));

        if (matchEnd && winnerId == matchConfigSO.UserId)
        {
            var saver = new JsonMasterySaver();
            new StageProgressUseCase(new PlayerPrefsStageStorage(), saver.Load(), saver, matchConfigSO.EarnPointByStage).ClearStage(ai_id);
        }

        GameEndButtonModel CreateGameEndButtonModel(bool isGameEnd)
        {
            if (isGameEnd) return new GameEndButtonModel("로비로", () => SceneLoadHelper.LoadScene(SceneType.Lobby));
            else return new GameEndButtonModel("다음게임", () => SceneLoadHelper.LoadScene(SceneType.Battle));
        }
    }
}
