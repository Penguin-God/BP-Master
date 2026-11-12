using UnityEngine;

public static class SlotHighlightPalette
{
    // 외곽 프레임 및 상태별 색상 (알파 100% 고정)
    public static readonly Color DefaultBorder = new Color(0.82f, 0.82f, 0.85f, 1f);
    public static readonly Color DeadTint = new Color(0.45f, 0.45f, 0.45f, 1f);
    public static readonly Color UntargetableTint = new Color(0.35f, 0.35f, 0.38f, 1f);
    public static readonly Color TargetableBorder = new Color(0.55f, 1.0f, 0.2f, 1f);

    // 공격 관련
    public static readonly Color AttackCaster = new Color(1.0f, 0.45f, 0.1f, 1f);
    public static readonly Color AttackHit = AttackCaster;
    public static readonly Color AttackTarget = new Color(0.9f, 0.15f, 0.15f, 1f);

    // 스킬 관련
    public static readonly Color SkillCaster = new Color(0.2f, 0.6f, 1.0f, 1f);
    public static readonly Color SkillImpact = SkillCaster;
    public static readonly Color SkillTarget = new Color(0.2f, 0.9f, 0.4f, 1f);

    // 기본 상태 복원용
    public static readonly Color DefaultColor = Color.white;

    // 모션 주기 상수
    public const float PulsePeriod = 0.6f;
    public const float FlashDuration = 0.35f;
}
