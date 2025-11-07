using System.Collections.Generic;
using System.Linq;

public class PrioritySelector : IBanSelector, IPickSelector
{
    private readonly int[] banPlan;
    private readonly int[] pickPlan;

    public PrioritySelector(int[] banPlan, int[] pickPlan)
    {
        this.banPlan = banPlan ?? new int[0];
        this.pickPlan = pickPlan ?? new int[0];
    }

    public int Pick(HashSet<int> selectableIds)
    {
        // 1) 우선순위 순서대로 가능한 후보 탐색
        foreach (int id in pickPlan)
        {
            if (selectableIds.Contains(id))
                return id;
        }

        // 2) 모든 후보가 불가능하면 랜덤
        return RandomUtil.DrawRandom(selectableIds);
    }

    public int Ban(HashSet<int> selectableIds)
    {
        // 1) 우선순위 순서대로 가능한 후보 탐색
        foreach (int id in banPlan)
        {
            if (selectableIds.Contains(id))
                return id;
        }

        selectableIds = selectableIds.Except(pickPlan).ToHashSet();
        return RandomUtil.DrawRandom(selectableIds);
    }
}
