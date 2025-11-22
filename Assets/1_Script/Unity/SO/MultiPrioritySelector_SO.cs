using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiPrioritySelector", menuName = "BP Master/MultiPrioritySelector")]
public class MultiPrioritySelector_SO : ScriptableObject
{
    [SerializeField] BuildPrioritySO[] buildDatas;

    public MultiPrioritySelector CraeteSeletor(MasteryManager masteryManager) => new MultiPrioritySelector(masteryManager, buildDatas.Select(x => x.CreateSelector()));
}
