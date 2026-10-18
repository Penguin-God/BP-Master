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
    public int Evaluate(Champion myCandidate)
    {
        evaluator.Change(myTeam, currentBoard);
        int myPickValue = evaluator.Evaluate(myCandidate);

        var nextFlow = phaseAdvancer.PeekNextFlow();

        if (nextFlow.Phase != GamePhase.Pick) // 픽 페이즈가 끝나면 1번만
            return myPickValue;

        var futureBoard = SkillPreviewer.PreviewSkill(myTeam, myCandidate, currentBoard);
        futureBoard.AddSlot(myTeam, myCandidate.Status.DeepCopy());
        Team nextTurnTeam = nextFlow.Turn;

        evaluator.Change(nextTurnTeam, futureBoard);

        int maxNextValue = 0;
        foreach (var champ in catalog.Champions.Where(x => x.Id != myCandidate.Id))
        {
            int value = evaluator.Evaluate(champ);

            if (value > maxNextValue)
                maxNextValue = value;
        }

        evaluator.Change(myTeam, currentBoard);

        if (nextTurnTeam == myTeam)
            return myPickValue + maxNextValue;
        else
            return myPickValue - maxNextValue;
    }
}