using UnityEngine;


[CreateAssetMenu(fileName = "BuildPrioritySO", menuName = "AI/Selector/Random")]
public class RandomSelectorFactory : AI_SelectorSO
{
    public override IChampionSelector CreateBanSelector() => new RandomSelector();
    public override IChampionSelector CreatePickSelector() => new RandomSelector();
}
