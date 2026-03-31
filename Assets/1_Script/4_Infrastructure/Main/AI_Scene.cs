using Match;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Scene : MonoBehaviour
{
    [SerializeField] int ai_id1;
    [SerializeField] int ai_id2;
    [SerializeField] AIFactorySO aiFactory;
    [SerializeField] int matchCount;
    [SerializeField] BonusDataFactory bonusDataSO;
    [SerializeField] MatchCoreFactorySO matchCoreFactorySO;

    PickSlotFacade PickSlotFacade;
    Dictionary<Team, int> idByTeam = new();

    void Awake()
    {
        idByTeam.Add(Team.Blue, ai_id1);
        idByTeam.Add(Team.Red, ai_id2);
        StartMatch();
    }

    void StartMatch()
    {
        MatchContext.MatchInit(new MatchData(ai_id1, ai_id2), 2, ChampionDataLoder.AllId);
        StartBattle(MatchContext.Storage);
    }

    void StartBattle(BanPickStorage storage)
    {
        var catalog = ChampionDataLoder.GetCatalog();
        var core = matchCoreFactorySO.CreateMatchCore(storage, catalog, idByTeam);

        var blueEntry = CreateEntry(Team.Blue, ai_id1);
        var redEntry = CreateEntry(Team.Red, ai_id2);
        core.SetupPhaseManager(blueEntry, redEntry);

        PickSlotFacade = core.BanPickHandler.PickSlotFacade;
        core.PhaseManager.OnGameEnd += OnDone;
        core.PhaseManager.Start();

        AI_Entry CreateEntry(Team team, int id) => new AI_Entry(team, id, aiFactory, storage, core.SkillController, catalog, core.MasteryRegistry, core.BanPickHandler, core.PhaseAdvancer);
    }

    int blueWin;
    int redWin;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);

        if (result.Winner == Team.All)
        {
            print("이게 무승부네 ㅋㅋ");
            StartBattle(MatchContext.Storage);
        }

        if (result.Winner == Team.Blue) blueWin++;
        else redWin++;

        StartCoroutine(Co_EndMatch(idByTeam[result.Winner], result.Winner));
    }

    int blueMatchWin;
    int redMatchWin;
    IEnumerator Co_EndMatch(int winnerId, Team winTeam)
    {
        yield return null;

        if (MatchContext.EndMatch(winnerId))
        {
            if (winTeam == Team.Blue) blueMatchWin++;
            else redMatchWin++;

            matchCount--;
            if (matchCount > 0) StartMatch();
            else
            {
                print($"blue win : {blueWin}");
                print($"red win : {redWin}");

                print($"블루 매치 승 : {blueMatchWin}");
                print($"레드 매치 승 : {redMatchWin}");
            }
        }
        else StartBattle(MatchContext.Storage);
    }
}

public class AI_Entry : IPhaseEntry
{
    readonly Team Team;
    readonly AI_BanPickAgent banPickAgent;
    readonly AI_SkillExecutionUseCase skillUseCase;

    public void EnterBan() => banPickAgent.Ban(Team);
    public void EnterPick() => banPickAgent.Pick(Team);

    public AI_Entry(Team team, int ai_id, AIFactorySO aiFactory, BanPickStorage storage, SkillUsecase skillUseController, ChampionCatalog championCatalog, MasteryRegistry masteryRegistry, BanPickHandler banPickHandler, PhaseAdvancer phaseAdvancer)
    {
        Team = team;
        AI_SelectorFactory selectorFactory = aiFactory.CreateAI(ai_id, Team, storage, championCatalog, masteryRegistry, banPickHandler, phaseAdvancer);
        banPickAgent = new AI_BanPickAgent(Team, storage, selectorFactory.CreateBanSelector(), selectorFactory.CreatePickSelector(), banPickHandler);
        banPickHandler.BanPickEventDispatcher.OnPick += UseSkill;
        skillUseCase = new AI_SkillExecutionUseCase(banPickHandler.PickSlotFacade.SkillSlots, skillUseController, new SkillTargetService(new HighStatTargetSelector(banPickHandler.PickSlotFacade.StatusSlots)));
    }

    void UseSkill(SlotData slotData, int id)
    {
        if (slotData.Team != Team) return;
        skillUseCase.UseSkill(slotData);
    }
}