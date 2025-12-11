using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiPrioritySelector", menuName = "BP Master/MultiPrioritySelector")]
public class MultiPrioritySelector_SO : SelectorsCreatetorSO
{
    [SerializeField] BuildPrioritySO[] buildDatas;

    public override IBanSelector CreateBanSelector() => CreateSelector(masteryManager);
    public override IPickSelector CreatePickSelector() => CreateSelector(masteryManager);

    MultiPrioritySelector CreateSelector(MasteryCollection masteryManager) => new MultiPrioritySelector(masteryManager, buildDatas.Select(x => x.CreateSelector()));
}
