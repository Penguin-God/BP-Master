using System;
using System.Collections.Generic;
using System.Linq;

public class DraftManager
{
    public readonly DraftState State = new ();
    public event Action<DraftCommand> OnCommandApplied;   // UI 바인딩용

    public void ApplyCommand(DraftCommand cmd)
    {
        var turn = State.CurrentTurn;
        if (cmd.team != turn.team)
            throw new InvalidOperationException("차례가 아닙니다.");

        cmd.Execute(State);
        OnCommandApplied?.Invoke(cmd);

        // 턴 카운트 감소 및 큐 처리
        FinishSubTurn();
    }

    private void FinishSubTurn()
    {
        var turn = State.CurrentTurn;
        if (turn.count > 1)
        {
            // 남은 선택 수 줄이기
            State.turnQueue.Dequeue();
            State.turnQueue.Enqueue(new TurnDescriptor(turn.phase, turn.team, turn.count - 1));
        }
        else
        {
            // 이번 턴 완전히 끝남
            State.turnQueue.Dequeue();
        }

        // AI 차례라면 즉시 실행
        if (!State.IsFinished && State.CurrentTurn.team == Team.Red) // 예: Red를 AI로 가정
            PerformAIRandom();
    }

    private void PerformAIRandom()
    {
        var turn = State.CurrentTurn;
        var pool = GetRemainingPool();

        for (int i = 0; i < Math.Max(1, turn.count); i++)
        {
            int pick = pool[new Random().Next(pool.Count)];
            DraftCommand cmd = turn.phase switch
            {
                Phase.Ban => new BanCommand(Team.Red, pick),
                Phase.Pick => new PickCommand(Team.Red, pick),
                _ => throw new NotImplementedException()
            };
            ApplyCommand(cmd); // 재귀적으로 진행
        }
    }

    private List<int> GetRemainingPool()
    {
        // 실제 구현에서는 ChampionSO[] 풀과 매칭
        // 예시는 1~50 번 챔피언이라고 가정
        var all = Enumerable.Range(1, 50);
        return all.Where(id => !State.Banned.Contains(id)).ToList();
    }
}