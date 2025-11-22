using UnityEngine;

[CreateAssetMenu(fileName = "SelectorCreatetorSO", menuName = "Scriptable Objects/SelectorCreatetorSO")]
public abstract class SelectorsCreatetorSO : ScriptableObject
{
    protected MasteryManager masteryManager;
    public void Init(MasteryManager masteryManager) => this.masteryManager = masteryManager;

    public abstract IBanSelector CreateBanSelector();
    public abstract IPickSelector CreatePickSelector();
}
