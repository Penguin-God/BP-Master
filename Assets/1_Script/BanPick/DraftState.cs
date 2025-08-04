using System;
using System.Collections.Generic;

public enum Team { Blue, Red }
public enum Phase { Ban, Pick, Swap, Done }

public readonly struct TurnDescriptor
{
    public readonly Phase phase;
    public readonly Team team;
    public readonly int count; // 이번 턴에 수행해야 할 선택 수 (1 or 2)

    public TurnDescriptor(Phase p, Team t, int c = 1)
    {
        phase = p; team = t; count = c;
    }
}

// ──────────────────────────────────────────────────
// 2. DraftState – 현재 진행 상황을 보관
// ──────────────────────────────────────────────────
public class DraftState
{
    public readonly HashSet<int> Banned = new();
    public readonly Dictionary<Team, List<int>> Bans = new()
    {
        [Team.Blue] = new(),
        [Team.Red] = new()
    };
    public readonly Dictionary<Team, List<int>> Picks = new()
    {
        [Team.Blue] = new(),
        [Team.Red] = new()
    };

    public readonly Queue<TurnDescriptor> turnQueue = new();

    public DraftState()
    {
        // 2 밴 : 번갈아 1개씩
        turnQueue.Enqueue(new TurnDescriptor(Phase.Ban, Team.Blue));
        turnQueue.Enqueue(new TurnDescriptor(Phase.Ban, Team.Red));

        // 3 픽 : 1 → 2 → 2 → 1  (총 3개씩)
        Team firstPicker = Team.Blue;
        Team second = Team.Red;

        turnQueue.Enqueue(new TurnDescriptor(Phase.Pick, firstPicker, 1));
        turnQueue.Enqueue(new TurnDescriptor(Phase.Pick, second, 2));
        turnQueue.Enqueue(new TurnDescriptor(Phase.Pick, firstPicker, 2));
        turnQueue.Enqueue(new TurnDescriptor(Phase.Pick, second, 1));

        // 스왑 단계(선택적)
        turnQueue.Enqueue(new TurnDescriptor(Phase.Swap, Team.Blue, 0));
        turnQueue.Enqueue(new TurnDescriptor(Phase.Swap, Team.Red, 0));

        // 종료 마커
        turnQueue.Enqueue(new TurnDescriptor(Phase.Done, Team.Blue));
    }

    public TurnDescriptor CurrentTurn => turnQueue.Peek();
    public bool IsFinished => CurrentTurn.phase == Phase.Done;
}

public abstract class DraftCommand
{
    public readonly Team team;
    protected DraftCommand(Team t) => team = t;
    public abstract void Execute(DraftState state);
    public abstract void Undo(DraftState state);   // 필요 시
}

// ― 3-1 BanCommand
public class BanCommand : DraftCommand
{
    public readonly int champId;
    public BanCommand(Team t, int id) : base(t) { champId = id; }

    public override void Execute(DraftState s)
    {
        if (s.Banned.Contains(champId)) throw new InvalidOperationException("이미 밴/픽된 챔프입니다.");
        s.Banned.Add(champId);
        s.Bans[team].Add(champId);
    }
    public override void Undo(DraftState s)
    {
        s.Banned.Remove(champId);
        s.Bans[team].Remove(champId);
    }
}

// ― 3-2 PickCommand
public class PickCommand : DraftCommand
{
    public readonly int champId;
    public PickCommand(Team t, int id) : base(t) { champId = id; }

    public override void Execute(DraftState s)
    {
        if (s.Banned.Contains(champId)) throw new InvalidOperationException("이미 밴/픽된 챔프입니다.");
        s.Banned.Add(champId);
        s.Picks[team].Add(champId);
    }
    public override void Undo(DraftState s)
    {
        s.Banned.Remove(champId);
        s.Picks[team].Remove(champId);
    }
}

// ― 3-3 SwapCommand  (동일 팀 내에서 두 챔피언을 맞바꿈)
public class SwapCommand : DraftCommand
{
    readonly int indexA, indexB; // 0~2
    public SwapCommand(Team t, int idxA, int idxB) : base(t)
    { indexA = idxA; indexB = idxB; }

    public override void Execute(DraftState s)
    {
        var picks = s.Picks[team];
        if (picks.Count <= Math.Max(indexA, indexB))
            throw new InvalidOperationException("아직 픽이 완료되지 않은 슬롯입니다.");

        (picks[indexA], picks[indexB]) = (picks[indexB], picks[indexA]);
    }
    public override void Undo(DraftState s) => Execute(s); // 스왑은 동일 연산
}
