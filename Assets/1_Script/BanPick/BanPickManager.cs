using System.Collections.Generic;
using System.Linq;

public enum Team { Blue, Red}

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
    readonly Dictionary<Team, TeamSelectManager> selectManagerDict = new();
    public HashSet<ChampionSO> selectedChampions = new();

    public BanPickManager(DraftTurnSO banTurnSO, DraftTurnSO pickTurnSO)
    {
        banTurn = new Queue<Team>(banTurnSO.Turns);
        pickTurn = new Queue<Team>(pickTurnSO.Turns);
        selectManagerDict.Add(Team.Blue, blue);
        selectManagerDict.Add(Team.Red, red);
    }

    public bool TrySelect(ChampionSO champion, out SelectData selectData)
    {
        selectData = new SelectData();
        if (selectedChampions.Contains(champion)) return false;

        var phase = CurrentPhase;
        selectedChampions.Add(champion);
        if (CurrentPhase == BanPcikPhase.Ban)
        {
            var team = ProgressGame(banTurn);
            selectManagerDict[team].Ban(champion);
            selectData = new SelectData(champion, team, phase, selectManagerDict[team].Baned.Count);
            return true;
        }
        else if (CurrentPhase == BanPcikPhase.Pick)
        {
            var team = ProgressGame(pickTurn);
            selectManagerDict[team].Pick(champion);
            selectData = new SelectData(champion, team, phase, selectManagerDict[team].Picks.Count);
            return true;
        }
        else return false;
    }

    Team ProgressGame(Queue<Team> turns)
    {
        Team result = turns.Dequeue();
        if (turns.Count == 0) CurrentPhase++;
        return result;
    }
}
