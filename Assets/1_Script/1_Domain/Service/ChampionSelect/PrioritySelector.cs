using System.Collections.Generic;
using System.Linq;

public class PrioritySelector : IChampionSelector
{
    public readonly IEnumerable<int> Plan;

    public PrioritySelector(IEnumerable<int> plan)
    {
        this.Plan = plan;
    }

    public int Select(HashSet<int> selectableIds)
    {
        foreach (int id in Plan)
        {
            if (selectableIds.Contains(id))
                return id;
        }

        var pool = selectableIds.ToList();
        return RandomUtil.DrawRandom(pool);
    }
}
