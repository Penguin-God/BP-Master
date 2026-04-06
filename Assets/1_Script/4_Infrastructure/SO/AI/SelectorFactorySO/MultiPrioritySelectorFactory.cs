using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiPrioritySelector", menuName = "AI/Selector/MultiPriority")]
public class MultiPrioritySelector_SO : AI_SelectorSO
{
    [SerializeField] BuildPrioritySO[] buildDatas;

    public override IChampionSelector CreateBanSelector() => CreateSelector(masteryManager);
    public override IChampionSelector CreatePickSelector() => CreatePickSelector(masteryManager);

    MultiPrioritySelector CreateSelector(MasteryStatCollection masteryManager) => new MultiPrioritySelector(masteryManager, buildDatas.Select(x => x.BanSelector()));
    MultiPrioritySelector CreatePickSelector(MasteryStatCollection masteryManager) => new MultiPrioritySelector(masteryManager, buildDatas.Select(x => x.PickSelector()));
}
