using UnityEngine;

public class TraitTooltipTrigger : TooltipTrigger
{
    [SerializeField] MatchConfigSO matchConfigSO;
    protected override string BuildText()
    {
        var traitData = matchConfigSO.TraitConfig;

        string chargeText = $"돌진 : 돌진 특성을 가진 아군 공격력 {traitData.ChargeAttack}만큼 증가";
        string ampliyText = $"증폭 : 픽된 아군의 증가율 {traitData.AmpilyRate}증가";
        string garudText = $"가드 : 픽된 아군의 감소율 {traitData.GuardBonusRate}만큼 감소";
        string breakText = $"돌진 : 픽된 상대방의 감소율 {traitData.BreakRate}만큼 증가";

        return string.Join("\n", chargeText, ampliyText, garudText, breakText);
    }
}
