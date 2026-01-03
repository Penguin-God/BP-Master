using UnityEngine;

[CreateAssetMenu(fileName = "SelectorCreatetorSO", menuName = "Scriptable Objects/SelectorCreatetorSO")]
public abstract class AI_SelectorFactory : ScriptableObject
{
    protected MasteryCollection masteryManager;
    public void Init(MasteryCollection masteryManager) => this.masteryManager = masteryManager;

    public abstract IBanSelector CreateBanSelector();
    public abstract IPickSelector CreatePickSelector();
}
