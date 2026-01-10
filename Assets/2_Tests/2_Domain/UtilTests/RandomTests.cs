using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class RandomTests
{
    [Test]
    public void 컬랙션_안에_있는_요소를_반환()
    {
        var numbers = new int[] { 0, 1, 2, 3, 4 };
        HashSet<int> draws = new();
        for (int i = 0; i < 1000; i++)
        {
            int result = RandomUtil.DrawRandom(numbers);
            draws.Add(result);
            Assert.IsTrue(numbers.Contains(result));
        }

        Assert.AreEqual(numbers.Length, draws.Count); // 뽑은 수랑 원래 배열 길이가 같음
    }
}
