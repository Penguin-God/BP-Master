using System.Linq;

public class PredictivePickEvaluator : IChampionEvaluator
{
    readonly PickValueEvaluator evaluator;
    readonly BanPickStorage storage;
    readonly ChampionCatalog catalog;
    readonly SkillPreviewer previewer;

    readonly Team myTeam;
    readonly SlotStorage<ChampionStatus> currentBoard;

    public PredictivePickEvaluator(PickValueEvaluator evaluator,BanPickStorage storage, ChampionCatalog catalog, SkillPreviewer previewer, Team myTeam, SlotStorage<ChampionStatus> currentBoard)
    {
        this.evaluator = evaluator;
        this.storage = storage;
        this.catalog = catalog;
        this.previewer = previewer;
        this.myTeam = myTeam;
        this.currentBoard = currentBoard;
    }

    public int Evaluate(Champion myCandidate)
    {
        // 1. 내 관점(Blue)으로 평가기 세팅 후 가치 계산
        evaluator.Change(myTeam, currentBoard);
        int myPickValue = evaluator.Evaluate(myCandidate);
        // 2. 내가 픽을 완료한 미래의 가상 보드 생성
        var futureBoard = previewer.PreviewSkill(myTeam, myCandidate, currentBoard);
        futureBoard.AddSlot(myTeam, myCandidate.Status);

        // 3. 상대방(Red) 관점과 미래 보드로 평가기 상태 '교체'
        Team enemyTeam = EnumCaster.GetOppoentTeam(myTeam);
        evaluator.Change(enemyTeam, futureBoard);

        int maxEnemyValue = 0;
        var enemyCandidates = storage.SelectableIds.Where(id => id != myCandidate.Id);

        // 4. 상대가 선택할 수 있는 픽 중 가장 가치가 높은(나에게 뼈아픈) 점수 찾기
        foreach (var enemyId in enemyCandidates)
        {
            var enemyChamp = catalog.GetChampion(enemyId);
            int enemyValue = evaluator.Evaluate(enemyChamp);

            if (enemyValue > maxEnemyValue)
            {
                maxEnemyValue = enemyValue;
            }
        }

        // 5. 다음 챔피언 평가를 위해 상태를 다시 '내 관점'으로 원상 복구
        evaluator.Change(myTeam, currentBoard);

        return myPickValue - maxEnemyValue;
    }
}