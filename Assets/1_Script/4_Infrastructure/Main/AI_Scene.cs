using UnityEngine;

public class AI_Scene : MonoBehaviour
{
    [SerializeField] int ai_id1;
    [SerializeField] int ai_id2;
    [SerializeField] AIFactorySO aiFactory;
    [SerializeField] GamePhaseLoderSO gamePhaseLoderSO;

    PickSlotFacade PickSlotFacade => banPickHandler.PickSlotFacade;
    MasteryRegistry masteryRegistry = new();
    BanPickHandler banPickHandler;
    MatchFlowUsecase matchFlowUsecase;
    MatchRecord matchRecord;
    MatchManager matchManager;

    void Awake()
    {
        Team playerTeam = Team.Blue;
        //matchManager = FindAnyObjectByType<MatchManager>();
        //matchRecord = matchManager.Record;
        //matchFlowUsecase = new MatchFlowUsecase(matchRecord, playerTeam);
        var storage = new BanPickStorage(ChampionDataLoder.AllId);

        masteryRegistry.InitTeamMastery(playerTeam, new MasteryCollection(new ChampionMastery[] { }));
        masteryRegistry.InitTeamMastery(EnumCaster.GetOppoentTeam(playerTeam), new MasteryCollection(new ChampionMastery[] { }));

        var phaseEventDispatcher = new PhaseEventDispatcher();
        banPickHandler = new BanPickHandler(ChampionDataLoder.GetCatalog(), storage);
        var actionEventDispathcer = new BanPickEventDispatcher();
        banPickHandler.BanPickEventDispatcher.OnTeamChampionPick += ApplyMastery;
        var skillController = new SkillUsecase(PickSlotFacade.ChampionSlots, new SkillRunner(new SkillActionFactory(actionEventDispathcer, phaseEventDispatcher), new SkillCondtionFactory()));

        var phaseAdvancer = new PhaseAdvancer(gamePhaseLoderSO.LoadPhase());
        var phaseManager = new PhaseFlowOrchestrator(phaseAdvancer, phaseEventDispatcher, new TeamPhaseEntryDispatcher(CreateEntry(Team.Blue, ai_id1), CreateEntry(Team.Red, ai_id2)));

        phaseManager.OnGameEnd += OnDone;
        // phaseManager.OnGameEnd += matchManager.EndMatch;
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
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(PickSlotFacade.StatusSlots);
        // matchFlowUsecase.EndMatch(result.Winner);
        print(result.Winner);
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