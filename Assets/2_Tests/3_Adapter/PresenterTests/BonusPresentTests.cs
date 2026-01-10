using NUnit.Framework;
using System;
using System.Collections.Generic;

public class BonusThresholdPresenterTests
{
    BonusTextBulider CreatePresenter() => new BonusTextBulider();

    SortedDictionary<int, int> CreateThresholds(params (int threshold, int bonus)[] items)
    {
        var dict = new SortedDictionary<int, int>();
        foreach (var (threshold, bonus) in items)
            dict[threshold] = bonus;
        return dict;
    }

    [Test]
    public void 한줄_텍스트_생성()
    {
        var presenter = CreatePresenter();
        var thresholds = CreateThresholds((150, 35), (200, 44), (300, 11));

        string result = presenter.BuildLineText(thresholds);

        Assert.AreEqual("150이상 +35, 200이상 +44, 300이상 +11", result);
    }

    [Test]
    public void 전체_텍스트_생성()
    {
        var presenter = CreatePresenter();

        var attack = CreateThresholds((150, 35), (200, 44), (300, 11));
        var defense = CreateThresholds((150, 35), (200, 44), (300, 11));
        var speed = CreateThresholds((10, 35), (15, 44), (20, 11));

        string result = presenter.BuildBonusAllText(attack, defense, speed);

        string nl = Environment.NewLine;
        string expected =
            "보너스 점수(누적 X)" + nl +
            "공격력 : 150이상 +35, 200이상 +44, 300이상 +11" + nl +
            "방어력 : 150이상 +35, 200이상 +44, 300이상 +11" + nl +
            "속도 : 10이상 +35, 15이상 +44, 20이상 +11";

        Assert.AreEqual(expected, result);
    }
}
