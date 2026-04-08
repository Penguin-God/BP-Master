using System;

public class MatchCore
{
    public readonly MasteryRegistry MasteryRegistry;
    public readonly PhaseEventDispatcher PhaseEventDispatcher;
    public readonly PhaseAdvancer PhaseAdvancer;
    public readonly BanPickHandler BanPickHandler;
    public readonly SkillUsecase SkillController;
    readonly TeamBonusCalculator TeamBonusCalculator;
    public PhaseFlowOrchestrator PhaseManager { get; private set; }

    public event Action<MatchResult> OnGameFinished;
    public MatchCore(ChampionCatalog catalog, BanPickStorage storage, PhaseAdvancer phaseAdvancer, MasteryRegistry masteryRegistry, TeamBonusCalculator teamBonusCalculator)
    {
        MasteryRegistry = masteryRegistry;

        PhaseEventDispatcher = new PhaseEventDispatcher();
        PhaseAdvancer = phaseAdvancer;

        BanPickHandler = new BanPickHandler(catalog, storage);

        var actionEventDispatcher = new BanPickEventDispatcher();
        BanPickHandler.BanPickEventDispatcher.OnTeamChampionPick += ApplyMastery;

        SkillController = new SkillUsecase(
            BanPickHandler.PickSlotFacade.ChampionSlots,
            new SkillRunner(new SkillActionFactory(actionEventDispatcher, PhaseEventDispatcher), new SkillCondtionFactory())
        );
        TeamBonusCalculator = teamBonusCalculator;
    }

    public void SetupPhaseManager(IPhaseEntry blueEntry, IPhaseEntry redEntry)
    {
        PhaseManager = new PhaseFlowOrchestrator(PhaseAdvancer, PhaseEventDispatcher, new TeamPhaseEntryDispatcher(blueEntry, redEntry));

        SkillController.OnUseSkill += slot => PhaseManager.SubmitAction(slot.Team);
        BanPickHandler.BanPickEventDispatcher.OnTeamBan += (team, _) => PhaseManager.SubmitAction(team);

        PhaseEventDispatcher.OnPhaseDone += HandleGameEnd;
    }

    void ApplyMastery(Champion champion, Team team) => new MasteryApplier(MasteryRegistry.GetTeamMasteryCollection(team)).ApplyMastery(champion.Id, champion.Status);

    void HandleGameEnd()
    {
        var builder = new MatchResultBuilder(TeamBonusCalculator);
        MatchResult result = new MatchResultConverter(builder).ToResult(BanPickHandler.PickSlotFacade.StatusSlots);

        OnGameFinished?.Invoke(result);
    }
}