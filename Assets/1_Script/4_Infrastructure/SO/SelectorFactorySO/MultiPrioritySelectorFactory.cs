using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiPrioritySelector", menuName = "BP Master/MultiPrioritySelector")]
public class MultiPrioritySelector_SO : AI_SelectorFactory
{
    [SerializeField] BuildPrioritySO[] buildDatas;

    public override IChampionSelector CreateBanSelector() => CreateSelector(masteryManager);
    public override IChampionSelector CreatePickSelector() => CreatePickSelector(masteryManager);

    MultiPrioritySelector CreateSelector(MasteryCollection masteryManager) => new MultiPrioritySelector(masteryManager, buildDatas.Select(x => x.BanSelector()));
    MultiPrioritySelector CreatePickSelector(MasteryCollection masteryManager) => new MultiPrioritySelector(masteryManager, buildDatas.Select(x => x.PickSelector()));
}
