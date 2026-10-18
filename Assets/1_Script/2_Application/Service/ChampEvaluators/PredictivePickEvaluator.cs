using System.Linq;

public class PredictivePickEvaluator
{
    readonly PickValueEvaluator evaluator;
    readonly ChampionCatalog catalog;
    readonly PhaseAdvancer phaseAdvancer;

    readonly Team myTeam;
    readonly SlotStorage<ChampionStatus> currentBoard;

    public PredictivePickEvaluator(PickValueEvaluator evaluator, ChampionCatalog catalog, PhaseAdvancer phaseAdvancer, Team myTeam, SlotStorage<ChampionStatus> currentBoard)
    {
        this.evaluator = evaluator;
        this.catalog = catalog;
        this.phaseAdvancer = phaseAdvancer;
        this.myTeam = myTeam;
        this.currentBoard = currentBoard;
    }
    public int Evaluate(Champion evaluateCard)
    {
        int defaultValue = evaluator.Evaluate(evaluateCard, myTeam, currentBoard);
        var nextFlow = phaseAdvancer.PeekNextFlow();

        if (nextFlow.Phase != GamePhase.Pick) // 픽 페이즈가 끝나면 1번만
            return defaultValue;

        var futureBoard = SkillPreviewer.PreviewSkill(myTeam, evaluateCard, currentBoard);
        futureBoard.AddSlot(myTeam, evaluateCard.Status.DeepCopy());
        Team nextTurnTeam = nextFlow.Turn;

        int maxNextValue = 0;
        foreach (var champ in catalog.Champions.Where(x => x.Id != evaluateCard.Id))
        {
            int value = evaluator.Evaluate(champ, nextTurnTeam, futureBoard);

            if (value > maxNextValue)
                maxNextValue = value;
        }

        if (nextTurnTeam == myTeam)
            return defaultValue + maxNextValue;
        else
            return defaultValue - maxNextValue;
    }
}