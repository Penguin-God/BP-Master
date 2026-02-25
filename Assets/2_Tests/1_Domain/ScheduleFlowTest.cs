using NUnit.Framework;

public class ScheduleFlowTest
{
    ScheduleFlow CreateFlow()
    {
        var matches = new[]
        {
            new MatchData(1, 101),
            new MatchData(1, 102),
        };
        return new ScheduleFlow(matches);
    }

    [Test]
    public void Advance를_호출하면_다음_일정으로_넘어간다()
    {
        Assert.AreEqual(101, CreateFlow().Advance().Id2);
    }

    [Test]
    public void Peek은_상태를_변경하지_않고_다음_일정을_반환한다()
    {
        var flow = CreateFlow();

        Assert.AreEqual(101, flow.PeekMatch.Id2);
        Assert.AreEqual(101, flow.PeekMatch.Id2);
    }

    [Test]
    public void 마지막_일정을_진행하면_종료_상태가_된다()
    {
        var flow = CreateFlow();

        flow.Advance();
        flow.Advance();

        Assert.IsTrue(flow.IsFinished);
    }
}