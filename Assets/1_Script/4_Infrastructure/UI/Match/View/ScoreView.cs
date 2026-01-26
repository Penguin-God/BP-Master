using System.Linq;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] StatView blueScoreView;
    [SerializeField] StatView redScoreView;
    SlotStorage<ChampionStatus> statusSlots;

    public void Init(SlotStorage<ChampionStatus> statusSlots)
    {
        this.statusSlots = statusSlots;
    }

    readonly StatAggregator statAggregator = new StatAggregator();
    public void UpdateTeamScore(SlotStorage<ChampionStatus> statuses, Team team)
    {
        var stat = statAggregator.AggregateStat(statuses.GetTeam(team).Select(x => x.Stat));

        if (team == Team.Blue) blueScoreView.UpdateStat(stat);
        else if (team == Team.Red) redScoreView.UpdateStat(stat);
    }

    void Update()
    {
        if (statusSlots == null) return;

        UpdateTeamScore(statusSlots, Team.Blue);
        UpdateTeamScore(statusSlots, Team.Red);
    }
}
