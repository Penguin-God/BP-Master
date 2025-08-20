using System;
using System.Collections.Generic;
using System.Linq;

public class RandomSelector
{
    public int Ban(IEnumerable<Champion> champions) => champions.ToArray()[new Random().Next(champions.Count())].Id;
}
