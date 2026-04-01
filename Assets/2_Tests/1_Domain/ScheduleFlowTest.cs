using NUnit.Framework;

public class ScheduleFlowTest
{
    ScheduleFlow CreateFlow(int startIndex = 0)
    {
        var matches = new[]
        {
            new MatchData(1, 101),
            new MatchData(1, 102),
            new MatchData(2, 103)
        };
        return new ScheduleFlow(matches, startIndex);
    }

    [Test]
    public void 생성자로_넘긴_인덱스에서_시작해_일정을_진행한다()
    {
        var flow = CreateFlow(startIndex: 1);

        Assert.AreEqual(102, flow.Advance().Id2);
        Assert.AreEqual(2, flow.CurrentIndex);
    }

    [Test]
    public void 마지막_일정을_진행하면_종료_상태가_된다()
    {
        var flow = CreateFlow();

        flow.Advance();
        flow.Advance();
        flow.Advance(); // 3진행 (끝)

        Assert.IsTrue(flow.IsFinished);
    }
}