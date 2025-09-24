using System.Linq;

public class MatchResultConverter
{
    readonly MatchResultBuilder matchResultBuilder;
    public MatchResultConverter(MatchResultBuilder matchResultBuilder) => this.matchResultBuilder = matchResultBuilder;

    public MatchResult ToResult(SlotStorage<ChampionStatus> statuses)
    {
        var blueStats = statuses.GetTeam(Team.Blue).Select(x => x.Stat);
        var redStats = statuses.GetTeam(Team.Red).Select(x => x.Stat);
        return matchResultBuilder.CalculateResult(blueStats, redStats);
    }
}
