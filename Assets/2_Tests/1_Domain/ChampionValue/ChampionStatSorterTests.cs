using System.Linq;
using NUnit.Framework;

public class ChampionStatSorterTests
{
    [Test]
    public void 스탯별_내림차순_정렬_확인()
    {
        var champ1 = TestHelper.CreateChampion(id: 1, att: 10, def: 30, speed: 20);
        var champ2 = TestHelper.CreateChampion(id: 2, att: 30, def: 20, speed: 10);
        var champ3 = TestHelper.CreateChampion(id: 3, att: 20, def: 10, speed: 30);
        var champions = new[] { champ1, champ2, champ3 };
        var sut = new ChampionStatSorter();

        var attackSorted = sut.SortByStat(champions, StatType.Attack).ToArray();
        var defenseSorted = sut.SortByStat(champions, StatType.Defense).ToArray();
        var speedSorted = sut.SortByStat(champions, StatType.Speed).ToArray();

        Assert.AreEqual(2, attackSorted[0].Id);
        Assert.AreEqual(3, attackSorted[1].Id);
        Assert.AreEqual(1, attackSorted[2].Id);

        Assert.AreEqual(1, defenseSorted[0].Id);
        Assert.AreEqual(2, defenseSorted[1].Id);
        Assert.AreEqual(3, defenseSorted[2].Id);

        Assert.AreEqual(3, speedSorted[0].Id);
        Assert.AreEqual(1, speedSorted[1].Id);
        Assert.AreEqual(2, speedSorted[2].Id);
    }
}