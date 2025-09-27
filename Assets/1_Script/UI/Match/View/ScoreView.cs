using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] BonusDataFactory bonusData;
    [SerializeField] TextMeshProUGUI bonusInfo;
    [SerializeField] ChampionView blueScoreView;
    [SerializeField] ChampionView redScoreView;

    void Start()
    {
        bonusInfo.text = new BonusPresenter().BuildBonusAllText(bonusData.AttackBonus.BonusDatas, bonusData.DefenseBonus.BonusDatas, bonusData.SpeedBonus.BonusDatas);
    }

    public void UpdateTeamScore(SlotStorage<ChampionStatus> statuses, Team team)
    {
        var stat = new StatAggregator().AggregateStat(statuses.GetTeam(team).Select(x => x.Stat));

        if (team == Team.Blue) blueScoreView.UpdateStat(stat);
        else if (team == Team.Red) redScoreView.UpdateStat(stat);
    }
}
