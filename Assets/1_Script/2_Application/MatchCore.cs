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

    public MatchCore(
        MasteryRegistry masteryRegistry,
        PhaseEventDispatcher phaseEventDispatcher,
        PhaseAdvancer phaseAdvancer,
        BanPickHandler banPickHandler,
        SkillUsecase skillController,
        PhaseFlowOrchestrator phaseManager,
        TeamBonusCalculator teamBonusCalculator)
    {
        MasteryRegistry = masteryRegistry;
        PhaseEventDispatcher = phaseEventDispatcher;
        PhaseAdvancer = phaseAdvancer;
        BanPickHandler = banPickHandler;
        SkillController = skillController;
        PhaseManager = phaseManager;
        TeamBonusCalculator = teamBonusCalculator;
    }

    public void FinishGame(MatchResult result) => OnGameFinished?.Invoke(result);
}

public static class MatchResultCalculator
{
    public static MatchResult CalculateResult(TeamBonusCalculator teamBonusCalculator, SlotStorage<ChampionStatus> statuses)
    {
        var builder = new MatchResultBuilder(teamBonusCalculator);
        return new MatchResultConverter(builder).ToResult(statuses);
    }
}
