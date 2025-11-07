using NUnit.Framework;
using UnityEngine;

public class StatChangeTextBuildTests
{
    [Test]
    public void 스탯_변경_데이터_뷰모델로_변환()
    {
        StatChangePresenter sut = new(Color.green, Color.red);
        StatChangeData data = new StatChangeData(new ChampionStatData(20, 20, 0), new ChampionStatData(38, 15, 0));

        var result = sut.CreateViewModel(data);

        Assert.IsTrue(result.Attack.IsChange);
        Assert.AreEqual(Color.green, result.Attack.DeltaTextColor);
        Assert.AreEqual("+18", result.Attack.DeltaText);

        Assert.IsTrue(result.Defense.IsChange);
        Assert.AreEqual(Color.red, result.Defense.DeltaTextColor);
        Assert.AreEqual("-5", result.Defense.DeltaText);

        Assert.IsFalse(result.Speed.IsChange);
        Assert.AreEqual(Color.white, result.Speed.DeltaTextColor);
        Assert.AreEqual("", result.Speed.DeltaText);
    }
}
