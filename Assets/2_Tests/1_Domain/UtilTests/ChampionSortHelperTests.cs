using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using static TestHelper;

public class ChampionSortHelperTests
{
    List<Champion> CreateSut() => new List<Champion>
    {
        CreateChampion(id: 1, att: 10, def: 30),
        CreateChampion(id: 2, att: 30, def: 10),
        CreateChampion(id: 3, att: 20, def: 20),
    };

    [TestCase(StatType.Attack, new[] { 2, 3, 1 })]
    [TestCase(StatType.Defense, new[] { 1, 3, 2 })]
    public void 스탯별_내림차순_정렬시_올바른_ID_순서가_반환된다(StatType statType, int[] expectedIds)
    {
        var sut = CreateSut();

        var result = ChampionSortHelper.SortByStat(sut, statType).ToArray();

        CollectionAssert.AreEqual(expectedIds, result);
    }

    [TestCase(ChampionSortType.Default, new[] { 1, 2, 3 })]
    [TestCase(ChampionSortType.Attack, new[] { 2, 3, 1 })]
    [TestCase(ChampionSortType.Defense, new[] { 1, 3, 2 })]
    public void 정렬_탭_타입에_따라_올바른_ID_목록이_반환된다(ChampionSortType sortType, int[] expectedIds)
    {
        var sut = CreateSut();

        var result = ChampionSortHelper.GetTabIds(sut, sortType, new[] { 1, 2, 3 }).ToArray();

        CollectionAssert.AreEqual(expectedIds, result);
    }
}
