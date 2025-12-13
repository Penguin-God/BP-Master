using NUnit.Framework;
using static TestHelper;

public class TeamPhaseEntryDispatcherTests
{
    [Test]
    public void 게임_흐름에_따라_에이전트에게_진입_명령()
    {
        TestEntry blue = new TestEntry();
        TestEntry red = new TestEntry();
        var sut = new TeamPhaseEntryDispatcher(blue, red);

        sut.EnterPhase(CreateFlow(GamePhase.Ban, Team.Blue));
        sut.EnterPhase(CreateFlow(GamePhase.Pick, Team.Red));

        Assert.AreEqual(1, blue.Count);
        Assert.AreEqual(2, red.Count);
    }
}
