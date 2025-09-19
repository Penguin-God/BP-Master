using NUnit.Framework;
using UnityEngine;

public class BanPickPersentTests
{
    [Test]
    public void 현재_순서에_맞는_슬롯_반환()
    {
        var sut = new TeamSlotIndexr();

        Assert.AreEqual(0, sut.AllocateIndex(Team.Blue));
        Assert.AreEqual(0, sut.AllocateIndex(Team.Red));
        Assert.AreEqual(1, sut.AllocateIndex(Team.Red));
        Assert.AreEqual(1, sut.AllocateIndex(Team.Blue));
    }

    [Test]
    public void 스탯_변경_데이터_뷰모델로_변환()
    {
        StatChangePresenter sut = new(Color.green, Color.red);
        StatChangeData data = new StatChangeData(TestHelper.CreateBlueSlot(1), new ChampionStatData(20, 20, 0), new ChampionStatData(25, 15, 0));

        var result = sut.CreateViewModel(data);

        Assert.IsTrue(result.Attack.IsChange);
        Assert.AreEqual(Color.green, result.Attack.DeltaTextColor);
        Assert.AreEqual("+5", result.Attack.DeltaText);
        CollectionAssert.AreEqual(new int[] { 21, 22, 23, 24, 25 }, result.Attack.DeltaValues);

        Assert.IsTrue(result.Defense.IsChange);
        Assert.AreEqual(Color.red, result.Defense.DeltaTextColor);
        Assert.AreEqual("-5", result.Defense.DeltaText);
        CollectionAssert.AreEqual(new int[] { 19, 18, 17, 16, 15 }, result.Defense.DeltaValues);

        Assert.IsFalse(result.Speed.IsChange);
        Assert.AreEqual(Color.white, result.Speed.DeltaTextColor);
        Assert.AreEqual("", result.Speed.DeltaText);
        CollectionAssert.AreEqual(null, result.Speed.DeltaValues);
    }
}
