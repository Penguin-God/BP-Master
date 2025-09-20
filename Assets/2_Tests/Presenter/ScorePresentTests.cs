using NUnit.Framework;
using System;

public class ScorePresentTests
{
    [Test]
    public void 점수_텍스트_생성()
    {
        ScorePresenter sut = new();
        TeamScoreInfo info = new(10, 15, 3, 5, 6);
        string result = sut.BuildText(info);

        string expected =
            "총점" + Environment.NewLine +
            "기본 점수 : 10 + 15 = 25" + Environment.NewLine +
            "보너스 점수 : 3 + 5 + 6 = 14" + Environment.NewLine +
            "최종 점수 : 25 + 14 = 39";

        Assert.AreEqual(expected, result);
    }
}
