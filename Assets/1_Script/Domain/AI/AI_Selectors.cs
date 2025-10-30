using System;
using System.Collections.Generic;
using System.Linq;

public interface IBanSelector
{
    public int Ban(HashSet<int> ids);
}

public interface IPickSelector
{
    public int Pick(HashSet<int> ids);
}

public interface IAI_Selector
{
    public int Ban(HashSet<int> ids);
    public int Pick(HashSet<int> ids);
}

public class RandomSelector : IAI_Selector
{
    readonly Random random = new Random();

    public int Ban(HashSet<int> ids) => RandomSelect(ids);
    public int Pick(HashSet<int> ids) => RandomSelect(ids);
    int RandomSelect(HashSet<int> ids) => ids.ToArray()[random.Next(ids.Count)];
}

public class RandomBan : IBanSelector
{
    public int Ban(HashSet<int> ids) => RandomUtil.DrawRandom(ids);
}

public class RandomPick : IPickSelector
{
    public int Pick(HashSet<int> ids) => RandomUtil.DrawRandom(ids);
}
