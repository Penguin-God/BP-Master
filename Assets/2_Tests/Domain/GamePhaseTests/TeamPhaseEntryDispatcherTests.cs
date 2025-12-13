using NUnit.Framework;
using static TestHelper;

public class TeamPhaseEntryDispatcherTests
{
    public class TestEntry : IPhaseEntry
    {
        public int Count = 0;

        public void EnterBan() => Count++;
        public void EnterPick() => Count += 2;
    }

    [Test]
    public void 게임_흐름에_따라_에이전트에게_진입_명령()
    {
        TestEntry blue = new TestEntry();
        TestEntry red = new TestEntry();
        var sut = new TeamPhaseEntryDispatcher(blue, red);

        sut.EnterPhase(CreateFlow(Team.Blue, GamePhase.Ban));
        sut.EnterPhase(CreateFlow(Team.Red, GamePhase.Pick));

        Assert.AreEqual(1, blue.Count);
        Assert.AreEqual(2, red.Count);
    }
}
