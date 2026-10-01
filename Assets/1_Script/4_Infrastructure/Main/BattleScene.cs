using Match;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] MatchUI_Controller matchUI_Controller;
    [SerializeField] AI_Main ai_main;
    [SerializeField] ChampionSelector_UI championSelector;

    Dictionary<Team, int> playerIds = new();
    [SerializeField] MatchCoreFactorySO matchCoreFactorySO;
    [SerializeField] MatchConfigSO matchConfigSO;
    [SerializeField] TutorialTriggerSO tutorialTriggerSO;

    int ai_id;
    BanPickStorage storage;
    public void GameStart(Team playerTeam)
    {
        ai_id = MatchContext.MatchState.GetOpponentId(matchConfigSO.UserId);
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerIds.Add(playerTeam, matchConfigSO.UserId);
        playerIds.Add(aiTeam, ai_id);

        var championCatalog = ChampionDataLoder.GetCatalog();
        storage = MatchContext.CreateFearlessStorage();
        var core = matchCoreFactorySO.CreateMatchCore(storage, championCatalog, playerIds);

        var masteryRegistry = core.MasteryRegistry;

        var (blue, red) = CreatePhaseOrchestrator(championSelector, ai_main, playerTeam);
        core.SetupPhaseManager(blue, red);
        
        matchUI_Controller.Init(playerTeam, core, playerIds); // start보다 먼저

        ai_main.Init(ai_id, aiTeam, storage, core.SkillController, championCatalog, masteryRegistry, core.BanPickHandler, core.PhaseAdvancer);

        TutorialEventBinder.BindBattleTutorial(tutorialTriggerSO.StartTutorialOneTime, MatchContext.MatchState.TotalWins);

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
        bool matchEnd = false;
        if (result.Winner == Team.All) ; // 승자는 후픽으로 설정하기
        else
        {
            MatchContext.RecordMatchResult(storage.PickIds.GetAll());
            matchEnd = MatchContext.EndMatch(playerIds[result.Winner]);
        }

        Object.FindAnyObjectByType<MatchResultView>(FindObjectsInactive.Include).DrawResult(result, CreateGameEndButtonModel(matchEnd));

        if (matchEnd && playerIds[result.Winner] == matchConfigSO.UserId)
        {
            var saver = new JsonMasterySaver();
            new StageProgressUseCase(new PlayerPrefsStageStorage(), saver.Load(), saver, matchConfigSO.EarnPointByStage).ClearStage(ai_id);
        }

        GameEndButtonModel CreateGameEndButtonModel(bool isGameEnd)
        {
            if (isGameEnd) return new GameEndButtonModel("로비로", () => SceneLoadHelper.LoadScene(SceneType.Lobby));
            else return new GameEndButtonModel("스왑", () => SceneLoadHelper.LoadScene(SceneType.Swap));
        }
    }
}