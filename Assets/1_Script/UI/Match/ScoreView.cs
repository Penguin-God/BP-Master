using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] BonusDataFactory bonusData;
    [SerializeField] TextMeshProUGUI bonusInfo;
    [SerializeField] ChampionView blueScoreView;
    [SerializeField] ChampionView redScoreView;
    SlotStorage<ChampionStatus> picks;
    void Start()
    {
        bonusInfo.text = new BonusPresenter().BuildBonusAllText(bonusData.AttackBonus.BonusDatas, bonusData.DefenseBonus.BonusDatas, bonusData.SpeedBonus.BonusDatas);
    }

    public void Init(SlotStorage<ChampionStatus> picks) => this.picks = picks;

    DefaultScoreCalculator scoreCalculator = new DefaultScoreCalculator();
    public void UpdateTeamScore(Team team)
    {
        int att = scoreCalculator.CalculateAttack(picks.GetTeam(team).Select(x => x.Stat));
        int def = scoreCalculator.CalculateDefense(picks.GetTeam(team).Select(x => x.Stat));
        int speed = picks.GetTeam(team).Sum(x => x.Stat.Speed);

        if(team == Team.Blue) blueScoreView.UpdateStat(new ChampionStatData(att, def, speed));
        else redScoreView.UpdateStat(new ChampionStatData(att, def, speed));
    }
}
