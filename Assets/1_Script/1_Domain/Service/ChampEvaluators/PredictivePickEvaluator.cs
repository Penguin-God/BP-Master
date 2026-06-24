using System.Linq;

public class PredictivePickEvaluator : IChampionEvaluator
{
    readonly PickValueEvaluator evaluator;
    readonly BanPickStorage storage;
    readonly ChampionCatalog catalog;
    readonly PhaseAdvancer phaseAdvancer;

    readonly Team myTeam;
    readonly SlotStorage<ChampionStatus> currentBoard;

    public PredictivePickEvaluator(PickValueEvaluator evaluator,BanPickStorage storage, ChampionCatalog catalog, PhaseAdvancer phaseAdvancer, Team myTeam, SlotStorage<ChampionStatus> currentBoard)
    {
        this.evaluator = evaluator;
        this.storage = storage;
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
        var candidates = storage.SelectableIds.Where(id => id != myCandidate.Id);

        foreach (var id in candidates)
        {
            var champ = catalog.GetChampion(id);
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