using UnityEngine;


[CreateAssetMenu(fileName = "BuildPrioritySO", menuName = "BP Master/RandomSO")]
public class RandomSelectorFactory : AI_SelectorFactory
{
    public override IChampionSelector CreateBanSelector() => new RandomSelector();
    public override IChampionSelector CreatePickSelector() => new RandomSelector();
}
