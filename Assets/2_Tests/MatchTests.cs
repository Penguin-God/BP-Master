using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void 매치는_순서대로_진행()
    {
        MatchManager sut = new();

        sut.GameStart();

        Assert.AreEqual(Team.Blue, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Ban, sut.CurrentPhase);

        blue.Ban();
        Assert.AreEqual(Team.Red, sut.CurrentTurn);

        red.Ban();
        Assert.AreEqual(Team.Blue, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Pick, sut.CurrentPhase);

        blue.Pick();
        red.Pick();

        Assert.AreEqual(Team.All, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Swap, sut.CurrentPhase);

        blue.SwapDone();
        Assert.AreEqual(GamePhase.Swap, sut.CurrentPhase);

        red.SwapDone();
        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }

    // 밴픽은 알아서 저장됨
}
