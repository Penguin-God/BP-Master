using System.Collections.Generic;

public struct LeaderboardDisplayModel
{
    public readonly int Rank;
    public readonly string TeamName;
    public readonly string WinText;
    public readonly string LoseText;
    public readonly string ScoreText;

    public LeaderboardDisplayModel(int rank, string teamName, string winText, string loseText, string scoreText)
    {
        Rank = rank;
        TeamName = teamName;
        WinText = winText;
        LoseText = loseText;
        ScoreText = scoreText;
    }
}

public class LeaderboardPresenter
{
    readonly LeagueRecordCollection _collection;
    readonly IPlayerDataLoader _dataLoader;

    public LeaderboardPresenter(LeagueRecordCollection collection, IPlayerDataLoader dataLoader)
    {
        _collection = collection;
        _dataLoader = dataLoader;
    }

    public IReadOnlyList<LeaderboardDisplayModel> GetDisplayData()
    {
        var result = new List<LeaderboardDisplayModel>();
        var sortedRecords = _collection.GetSortedLeaderboard();

        for (int i = 0; i < sortedRecords.Count; i++)
        {
            var record = sortedRecords[i];

            result.Add(CreateModel(i+1, record));
        }

        return result;
    }

    LeaderboardDisplayModel CreateModel(int rank, LeagueRecord record) 
        => new LeaderboardDisplayModel(
            rank,
            teamName: _dataLoader.LoadPlayer(record.Id).Name,
            winText: record.Win.ToString(),
            loseText: record.Lose.ToString(),
            scoreText: FormatScore(record.Score)
        );

    string FormatScore(int score)
    {
        if (score > 0) return $"+{score}";
        return score.ToString(); // 0 이거나 음수인 경우는 ToString()이 자연스럽게 처리(- 포함)
    }
}