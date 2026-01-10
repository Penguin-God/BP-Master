using System;
using System.Collections.Generic;
using System.Linq;

public class PrioritySelector : IBanSelector, IPickSelector
{
    readonly IEnumerable<int> banPlan;
    readonly IEnumerable<int> pickPlan;
    public IEnumerable<int> PickPlan => pickPlan;

    public PrioritySelector(IEnumerable<int> banPlan, IEnumerable<int> pickPlan)
    {
        this.banPlan = banPlan;
        this.pickPlan = pickPlan;
    }

    public int Pick(HashSet<int> selectableIds) => ChooseByPlanOrRandom(selectableIds, pickPlan, _ => true); 
    public int Ban(HashSet<int> selectableIds) => ChooseByPlanOrRandom(selectableIds, banPlan, id => pickPlan.Contains(id) == false);

    int ChooseByPlanOrRandom(HashSet<int> selectableIds, IEnumerable<int> plan, Func<int, bool> exclude)
    {
        foreach (int id in plan)
        {
            if (selectableIds.Contains(id))
                return id;
        }

        var pool = selectableIds.Where(id => exclude(id)).ToList();
        if (pool.Count == 0) pool = selectableIds.ToList(); 
        return RandomUtil.DrawRandom(pool);
    }
}
