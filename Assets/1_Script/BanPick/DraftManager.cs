using System;
using System.Collections.Generic;
using System.Linq;

public class DraftManager
{
    public readonly DraftState State = new ();
    public event Action<DraftCommand> OnCommandApplied;   // UI ���ε���

    public void ApplyCommand(DraftCommand cmd)
    {
        var turn = State.CurrentTurn;
        if (cmd.team != turn.team)
            throw new InvalidOperationException("���ʰ� �ƴմϴ�.");

        cmd.Execute(State);
        OnCommandApplied?.Invoke(cmd);

        // �� ī��Ʈ ���� �� ť ó��
        FinishSubTurn();
    }

    private void FinishSubTurn()
    {
        var turn = State.CurrentTurn;
        if (turn.count > 1)
        {
            // ���� ���� �� ���̱�
            State.turnQueue.Dequeue();
            State.turnQueue.Enqueue(new TurnDescriptor(turn.phase, turn.team, turn.count - 1));
        }
        else
        {
            // �̹� �� ������ ����
            State.turnQueue.Dequeue();
        }

        // AI ���ʶ�� ��� ����
        if (!State.IsFinished && State.CurrentTurn.team == Team.Red) // ��: Red�� AI�� ����
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
            ApplyCommand(cmd); // ��������� ����
        }
    }

    private List<int> GetRemainingPool()
    {
        // ���� ���������� ChampionSO[] Ǯ�� ��Ī
        // ���ô� 1~50 �� è�Ǿ��̶�� ����
        var all = Enumerable.Range(1, 50);
        return all.Where(id => !State.Banned.Contains(id)).ToList();
    }
}