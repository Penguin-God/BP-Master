using System;
using System.Text;

public class ScorePresenter
{
    public string BuildText(TeamScoreInfo info)
    {
        var line = Environment.NewLine;
        var sb = new StringBuilder();
        sb.Append("총점").Append(line);
        sb.Append($"기본 점수 : {info.AttackTotal} + {info.DefenseTotal} = {info.DefaultScore}").Append(line);
        sb.Append($"보너스 점수 : {info.AttackBonus} + {info.DefenseBonus} + {info.SpeedBonus} = {info.BonusScore}").Append(line);
        sb.Append($"최종 점수 : {info.DefaultScore} + {info.BonusScore} = {info.Total}");
        return sb.ToString();
    }
}
