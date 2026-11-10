using NUnit.Framework;
using static TestHelper;

public class StatusSlotConvertTests
{
    [Test]
    public void 현재_상태를_점수로_반환()
    {
        var statusSlots = CreateTwoSlotStatus(att: 100, def: 50);

        GameScoreInfo result = ScoreConvertor.Convert(statusSlots);

        Assert.AreEqual(200, result.Blue.Att);
        Assert.AreEqual(100, result.Blue.Def);
        Assert.AreEqual(200, result.Red.Att);
        Assert.AreEqual(100, result.Red.Def);
    }
}
