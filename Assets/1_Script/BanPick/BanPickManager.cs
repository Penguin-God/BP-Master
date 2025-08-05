using System.Collections.Generic;

public enum Team { Blue, Red }

public class TeamSelectStorage
{
    readonly List<ChampionSO> bans = new();
    public IReadOnlyList<ChampionSO> Baned => bans;
    readonly List<ChampionSO> picks = new();
    public IReadOnlyList<ChampionSO> Picks => picks;

    public void Ban(ChampionSO championSO) => bans.Add(championSO);
    public void Pick(ChampionSO championSO) => picks.Add(championSO);
}

public readonly struct SelectData
{
    public readonly ChampionSO Champion;
    public readonly TurnInfo CurrentTurn;
    public readonly TurnInfo NextTurn;
    public readonly int Count;

    public SelectData(ChampionSO champion, TurnInfo currentTurn, TurnInfo nextTurn, int count)
    {
        Champion = champion;
        CurrentTurn = currentTurn;
        NextTurn = nextTurn;
        Count = count;
    }
}

public readonly struct TurnInfo
{
    public readonly Team Team;
    public readonly BanPcikPhase Phase;

    public TurnInfo(Team team, BanPcikPhase phase)
    {
        Team = team;
        Phase = phase;
    }
}

public class BanPickManager
{
    public BanPcikPhase CurrentPhase => turnQueue.Count == 0 ? BanPcikPhase.Swap : turnQueue.Peek().Phase;

    readonly Queue<TurnInfo> turnQueue = new();
    readonly Dictionary<Team, TeamSelectStorage> storageDict = new();

    readonly HashSet<ChampionSO> selected = new();

    public BanPickManager(DraftTurnSO banTurnSO, DraftTurnSO pickTurnSO)
    {
        // ban, pick 순서대로 큐에 삽입
        foreach (var team in banTurnSO.Turns) turnQueue.Enqueue(new TurnInfo(team, BanPcikPhase.Ban));
        foreach (var team in pickTurnSO.Turns) turnQueue.Enqueue(new TurnInfo(team, BanPcikPhase.Pick));

        storageDict.Add(Team.Blue, new TeamSelectStorage());
        storageDict.Add(Team.Red, new TeamSelectStorage());
    }

    public bool TrySelect(ChampionSO champ, out SelectData data)
    {
        data = default;
        if (selected.Contains(champ) || turnQueue.Count == 0) return false;

        var turn = turnQueue.Dequeue();
        var storage = storageDict[turn.Team];
        var phase = turn.Phase;

        // 행동 실행
        selected.Add(champ);
        if (phase == BanPcikPhase.Ban) storage.Ban(champ);
        else if (phase == BanPcikPhase.Pick) storage.Pick(champ);

        // SelectData 작성
        int count = phase == BanPcikPhase.Ban ? storage.Baned.Count : storage.Picks.Count;

        data = new SelectData(champ, new TurnInfo(turn.Team, phase), GetNextTurnInfo(), count);
        return true;
    }

    TurnInfo GetNextTurnInfo() => turnQueue.Count == 0 ? new TurnInfo(0, BanPcikPhase.Swap) : turnQueue.Peek();
}
