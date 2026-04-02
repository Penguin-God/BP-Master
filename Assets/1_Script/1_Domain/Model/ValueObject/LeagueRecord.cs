using System.Collections.Generic;
using System.Linq;

public struct LeagueRecord
{
    public readonly int Id;
    public readonly int Win;
    public readonly int Lose;
    public readonly int Score;

    public LeagueRecord(int id, int win = 0, int lose = 0, int score = 0)
    {
        Id = id;
        Win = win;
        Lose = lose;
        Score = score;
    }

    public LeagueRecord ApplyMatchResult(int myWins, int opponentWins)
    {
        int newWin = Win;
        int newLose = Lose;

        if (myWins > opponentWins) newWin++;
        else if (myWins < opponentWins) newLose++;

        int newScore = Score + (myWins - opponentWins);
        return new LeagueRecord(Id, newWin, newLose, newScore);
    }
}

public class LeagueRecordCollection
{
    readonly Dictionary<int, LeagueRecord> _records;

    public LeagueRecordCollection(IEnumerable<LeagueRecord> records) => _records = records.ToDictionary(r => r.Id);
    public LeagueRecord Get(int id) => _records.TryGetValue(id, out var record) ? record : new LeagueRecord(id);
    public void Update(LeagueRecord record) => _records[record.Id] = record;

    public IEnumerable<LeagueRecord> GetAll() => _records.Values;

    public IReadOnlyList<LeagueRecord> GetSortedLeaderboard()
    {
        return _records.Values
            .OrderByDescending(r => r.Win)
            .ThenByDescending(r => r.Score)
            .ThenBy(r => r.Id)
            .ToList();
    }
}