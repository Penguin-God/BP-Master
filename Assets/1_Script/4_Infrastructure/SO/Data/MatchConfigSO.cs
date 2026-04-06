using UnityEngine;

[CreateAssetMenu(fileName = "MatchConfigSO", menuName = "BP Master/MatchConfigSO")]
public class MatchConfigSO : ScriptableObject
{
    public int UserId;
    public int TargetWinCount;
}
