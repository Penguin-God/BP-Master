using UnityEngine;

public static class SlotHighlightPalette
{
    // 공격 관련
    public static readonly Color AttackCaster = new Color(1.0f, 0.45f, 0.1f, 1f); // 주황빛
    public static readonly Color AttackTarget = new Color(0.9f, 0.15f, 0.15f, 1f); // 강한 붉은색

    // 스킬 관련
    public static readonly Color SkillCaster = new Color(0.2f, 0.6f, 1.0f, 1f);  // 푸른빛
    public static readonly Color SkillTarget = new Color(0.2f, 0.9f, 0.4f, 1f);  // 녹색/청록빛

    // 기본 상태 복원용
    public static readonly Color DefaultColor = Color.white;
}
