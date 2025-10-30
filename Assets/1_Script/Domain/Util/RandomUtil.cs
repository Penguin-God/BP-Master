using System;
using System.Collections.Generic;
using System.Linq;

public static class RandomUtil
{
    readonly static Random random = new Random();
    public static T PickRandom<T>(IEnumerable<T> source)
    {
        var list = source.ToList();
        int index = random.Next(list.Count);
        return list[index];
    }
}
