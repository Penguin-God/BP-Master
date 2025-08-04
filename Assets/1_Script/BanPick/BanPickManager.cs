using System.Collections.Generic;
using System.Linq;

public class TeamSelectManager
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
    public readonly Team Team;
    public readonly BanPcikPhase BanPcikPhase;
    public readonly int Count;

    public SelectData(ChampionSO champion, Team team, BanPcikPhase phase, int count)
    {
        Champion = champion;
        Team = team;
        BanPcikPhase = phase;
        Count = count;
    }
}

public class BanPickManager
{
    public BanPcikPhase CurrentPhase {  get; private set; } = BanPcikPhase.Ban;
    Queue<Team> banTurn;
    Queue<Team> pickTurn;

    TeamSelectManager blue = new();
    TeamSelectManager red = new();
    readonly Dictionary<Team, TeamSelectManager> selectManagerDick = new();

    public BanPickManager(DraftTurnSO banTurnSO, DraftTurnSO pickTurnSO)
    {
        banTurn = new Queue<Team>(banTurnSO.Turns);
        pickTurn = new Queue<Team>(pickTurnSO.Turns);
        selectManagerDick.Add(Team.Blue, blue);
        selectManagerDick.Add(Team.Red, red);
    }

    public bool TrySelect(ChampionSO champion, out SelectData selectData)
    {
        selectData = new SelectData();
        if (SelectConditoin(champion) == false) return false;

        var phase = CurrentPhase;
        if (CurrentPhase == BanPcikPhase.Ban)
        {
            var team = ProgressGame(banTurn);
            selectManagerDick[team].Ban(champion);
            selectData = new SelectData(champion, team, phase, selectManagerDick[team].Baned.Count);
            return true;
        }
        else if (CurrentPhase == BanPcikPhase.Pick)
        {
            var team = ProgressGame(pickTurn);
            selectManagerDick[team].Pick(champion);
            selectData = new SelectData(champion, team, phase, selectManagerDick[team].Picks.Count);
            return true;
        }
        else return false;
    }

    bool SelectConditoin(ChampionSO champion) => blue.Baned.Contains(champion) == false && blue.Picks.Contains(champion) == false && red.Baned.Contains(champion) == false && red.Picks.Contains(champion) == false;

    Team ProgressGame(Queue<Team> turns)
    {
        Team result = turns.Dequeue();
        if (turns.Count == 0) CurrentPhase++;
        return result;
    }
}
