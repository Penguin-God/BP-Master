using System.Linq;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] StatView blueScoreView;
    [SerializeField] StatView redScoreView;

    public void UpdateTeamScore(SlotStorage<ChampionStatus> statuses, Team team)
    {
        var stat = new StatAggregator().AggregateStat(statuses.GetTeam(team).Select(x => x.Stat));

        if (team == Team.Blue) blueScoreView.UpdateStat(stat);
        else if (team == Team.Red) redScoreView.UpdateStat(stat);
    }
}
