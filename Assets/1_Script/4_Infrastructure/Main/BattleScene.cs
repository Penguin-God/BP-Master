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
    public void GameStart(Team playerTeam)
    {
        ai_id = MatchContext.CurrentMatch.GetOpponentId(matchConfigSO.UserId);
        Team aiTeam = EnumCaster.GetOppoentTeam(playerTeam);

        playerIds.Add(playerTeam, matchConfigSO.UserId);
        playerIds.Add(aiTeam, ai_id);

        var championCatalog = ChampionDataLoder.GetCatalog();
        var storage = MatchContext.Storage;
        var core = matchCoreFactorySO.CreateMatchCore(storage, championCatalog, playerIds);

        var masteryRegistry = core.MasteryRegistry;

        var (blue, red) = CreatePhaseOrchestrator(championSelector, ai_main, playerTeam);
        core.SetupPhaseManager(blue, red);
        
        matchUI_Controller.Init(playerTeam, core, playerIds); // start보다 먼저

        ai_main.Init(ai_id, aiTeam, storage, core.SkillController, championCatalog, masteryRegistry, core.BanPickHandler, core.PhaseAdvancer);

        core.OnGameFinished += OnDone;
        core.PhaseManager.Start();

        MatchTutorial();
    }

    void MatchTutorial()
    {
        if (MatchContext.WinCounter.TotalWins == 0) tutorialTriggerSO.StartTutorialOneTime(TutorialType.MatchStart);
        else tutorialTriggerSO.StartTutorialOneTime(TutorialType.SecondSetEnter);
    }

    (IPhaseEntry blue, IPhaseEntry red) CreatePhaseOrchestrator(IPhaseEntry player, IPhaseEntry ai, Team playerTeam)
    {
        IPhaseEntry blue = playerTeam == Team.Blue ? player : ai;
        IPhaseEntry red = playerTeam == Team.Red ? player : ai;
        return (blue, red);
    }

    void OnDone(MatchResult result)
    {
        bool matchEnd = false;
        if (result.Winner == Team.All) MatchContext.Draw();
        else matchEnd = MatchContext.EndMatch(playerIds[result.Winner]);

        FindAnyObjectByType<MatchResultView>(FindObjectsInactive.Include).DrawResult(result, CreateGameEndButtonModel(matchEnd));

        if (matchEnd && playerIds[result.Winner] == matchConfigSO.UserId)
        {
            var saver = new JsonMasterySaver();
            new StageProgressUseCase(new PlayerPrefsStageStorage(), saver.Load(), saver , matchConfigSO.EarnPointByStage).ClearStage(ai_id);
        }

        GameEndButtonModel CreateGameEndButtonModel(bool isGameEnd)
        {
            if (isGameEnd) return new GameEndButtonModel("로비로", () => SceneLoadHelper.LoadScene(SceneType.Lobby));
            else return new GameEndButtonModel("스왑", () => SceneLoadHelper.LoadScene(SceneType.Swap));
        }
    }
}