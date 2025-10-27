using UnityEngine;

[CreateAssetMenu(fileName = "MatchConfigSO", menuName = "BP Master/MatchConfigSO")]
public class MatchConfigSO : ScriptableObject
{
    [Header("기본 설정")]
    [SerializeField] int teamSize;
    public int TeamSize => teamSize;

    [Header("특성 보너스 설정")]
    [SerializeField] int chargeAttackBonus;
    public int ChargeAttackBonus => chargeAttackBonus;

    [SerializeField] float guardBonusRate;
    public float GuardBonusRate => guardBonusRate;

    [SerializeField] float amplifyBonusRate;
    public float AmplifyBonusRate => amplifyBonusRate;

    [SerializeField] float breakRate;

    public TraitConfig TraitConfig => new TraitConfig(chargeAttackBonus, guardBonusRate, amplifyBonusRate, breakRate);
}
