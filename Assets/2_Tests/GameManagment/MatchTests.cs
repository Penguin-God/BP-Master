using NUnit.Framework;
using System.Collections.Generic;

public class MatchTests
{
    [Test]
    public void All턴이면_모든_팀_호출()
    {
        var (blue, red, sut) = CreateArrange();

        sut.OnRequestAction(CreateData(GamePhase.Swap, Team.All));

        Assert.AreEqual(1, blue.Calls.Count);
        Assert.AreEqual(("Swap", Team.Blue), blue.Calls[0]);

        Assert.AreEqual(1, red.Calls.Count);
        Assert.AreEqual(("Swap", Team.Red), red.Calls[0]);
    }

    [Test]
    public void Done페이즈는_어떤_핸들러도_호출하지_않는다()
    {
        var (blue, red, sut) = CreateArrange();

        sut.OnRequestAction(CreateData(GamePhase.Done, Team.All));

        Assert.AreEqual(0, blue.Calls.Count);
        Assert.AreEqual(0, red.Calls.Count);
    }

    public static GameFlowData CreateData(GamePhase phase, Team turn) => new GameFlowData(phase, turn);

    (FakeActionHandler, FakeActionHandler, PhaseActionDispatcher) CreateArrange()
    {
        var blue = new FakeActionHandler();
        var red = new FakeActionHandler();
        var sut = new PhaseActionDispatcher(blue, red);
        return (blue, red, sut);
    }
}

public class FakeActionHandler : IActionHandler
{
    public readonly List<(string Method, Team Team)> Calls = new();
    public void OnRequestSwap(Team turnTeam) => Calls.Add(("Swap", turnTeam));
}