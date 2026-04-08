using System.Collections.Generic;
using System.Linq;

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
    readonly ILeagueRecordStorage _storage;
    readonly IPlayerDataLoader _dataLoader;

    public LeaderboardPresenter(ILeagueRecordStorage storage, IPlayerDataLoader dataLoader)
    {
        _storage = storage;
        _dataLoader = dataLoader;
    }

    public IReadOnlyList<LeaderboardDisplayModel> GetDisplayData() 
        => _storage.LoadAll()
            .GetSortedLeaderboard()
            .Select((record, index) => CreateModel(index + 1, record))
            .ToList();

    LeaderboardDisplayModel CreateModel(int rank, LeagueRecord record) 
        => new LeaderboardDisplayModel(
            rank,
            _dataLoader.LoadPlayer(record.Id).Name,
            winText: record.Win.ToString(),
            loseText: record.Lose.ToString(),
            scoreText: FormatScore(record.Score)
        );

    string FormatScore(int score)
    {
        if (score > 0) return $"+{score}";
        return score.ToString();
    }
}