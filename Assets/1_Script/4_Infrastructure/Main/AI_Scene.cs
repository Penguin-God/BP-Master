using Match;
using System.Collections;
using UnityEngine;

public class AI_Scene : MonoBehaviour
{
    [SerializeField] int ai_id1;
    [SerializeField] int ai_id2;
    [SerializeField] AIFactorySO aiFactory;
    [SerializeField] GamePhaseLoderSO gamePhaseLoderSO;
    [SerializeField] int matchCount;


    PickSlotFacade PickSlotFacade => banPickHandler.PickSlotFacade;
    MasteryRegistry masteryRegistry = new();
    BanPickHandler banPickHandler;

    void Awake()
    {
        StartMatch();
    }

    void StartMatch()
    {
        MatchContext.MatchInit(new MatchData(ai_id1, ai_id2), 2, new int[0], ChampionDataLoder.AllId);
        var storage = MatchContext.Storage;

        Team playerTeam = Team.Blue;
        masteryRegistry.InitTeamMastery(playerTeam, new MasteryCollection(new MasteryDrawer(storage.SelectableIds).DrawRandoms(new int[] { 15, 15, 15, 20, 20, 20, 20, 25, 25, 25 })));
        masteryRegistry.InitTeamMastery(EnumCaster.GetOppoentTeam(playerTeam), new MasteryCollection(new MasteryDrawer(storage.SelectableIds).DrawRandoms(new int[] { 15, 15, 15, 20, 20, 20, 20, 25, 25, 25 })));

        StartBattle(storage);
    }

    void StartBattle(BanPickStorage storage)
    {
        var phaseEventDispatcher = new PhaseEventDispatcher();
        banPickHandler = new BanPickHandler(ChampionDataLoder.GetCatalog(), storage);
        var actionEventDispathcer = new BanPickEventDispatcher();
        banPickHandler.BanPickEventDispatcher.OnTeamChampionPick += ApplyMastery;
        var skillController = new SkillUsecase(PickSlotFacade.ChampionSlots, new SkillRunner(new SkillActionFactory(actionEventDispathcer, phaseEventDispatcher), new SkillCondtionFactory()));

        var phaseAdvancer = new PhaseAdvancer(gamePhaseLoderSO.LoadPhase());
        var phaseManager = new PhaseFlowOrchestrator(phaseAdvancer, phaseEventDispatcher, new TeamPhaseEntryDispatcher(CreateEntry(Team.Blue, ai_id1), CreateEntry(Team.Red, ai_id2)));

        phaseManager.OnGameEnd += OnDone;
        skillController.OnUseSkill += slot => phaseManager.SubmitAction(slot.Team);
        banPickHandler.BanPickEventDispatcher.OnTeamBan += (team, _) => phaseManager.SubmitAction(team);

        phaseManager.Start();

        AI_Entry CreateEntry(Team team, int id) => new AI_Entry(team, id, aiFactory, storage, skillController, ChampionDataLoder.GetCatalog(), masteryRegistry, banPickHandler, phaseAdvancer);
    }

    void ApplyMastery(Champion champion, Team team)
    {
        var masteryApplier = new MasteryApplier(masteryRegistry.GetTeamMasteryManager(team));
        masteryApplier.ApplyMastery(champion.Id, champion.Status);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    int blueWin;
    int redWin;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);

        if(result.Winner == Team.All)
        {
            print("이게 무승부네 ㅋㅋ");
            StartBattle(MatchContext.Storage);
        }

        int winnerId = result.Winner == Team.Blue ? ai_id1 : ai_id2;

        if (result.Winner == Team.Blue) blueWin++;
        else redWin++;

        StartCoroutine(Co_EndMatch(winnerId, result.Winner));
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