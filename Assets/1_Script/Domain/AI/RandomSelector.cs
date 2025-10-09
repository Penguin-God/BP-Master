using System;

public class RandomSelector
{
    readonly Random random = new Random();
    public int Select(int[] ids) => ids[random.Next(ids.Length)];
}
