using System.Collections.Generic;
using System.Linq;
public class TraitTargetSelector
{
    readonly int teamCount;
    public TraitTargetSelector(int count) => teamCount = count;

    public IEnumerable<int> GetTargetIds(TargetRange range, int targetIndex)
    {
        switch (range)
        {
            case TargetRange.Single: return new int[] { targetIndex };
            case TargetRange.All: return Enumerable.Range(0, teamCount);
            default: return null;
        }
    }
}
