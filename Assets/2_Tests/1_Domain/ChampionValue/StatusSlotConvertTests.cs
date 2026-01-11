using NUnit.Framework;
using static TestHelper;

public class StatusSlotConvertTests
{
    [Test]
    public void 현재_상태를_점수로_반환()
    {
        var statusSlots = CreateTwoSlotStatus(att: 100, speed:2);

        GameScoreInfo result = ScoreConvertor.Convert(statusSlots);

        Assert.AreEqual(result.Blue.Att, 200);
        Assert.AreEqual(result.Blue.Def, 0);
        Assert.AreEqual(result.Red.Att, 200);
        Assert.AreEqual(result.Red.Speed, 4);
    }
}
