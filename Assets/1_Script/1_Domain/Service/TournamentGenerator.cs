using System;
using System.Collections.Generic;

public class TournamentGenerator
{
    public IEnumerable<MatchData> GenerateSemiFinals(IReadOnlyList<LeagueRecord> sortedLeaderboard)
    {
        if (sortedLeaderboard.Count < 4)
            throw new ArgumentException("토너먼트를 진행하려면 최소 4개의 팀이 필요합니다.");

        yield return new MatchData(sortedLeaderboard[0].Id, sortedLeaderboard[3].Id);
        yield return new MatchData(sortedLeaderboard[1].Id, sortedLeaderboard[2].Id);
    }
}