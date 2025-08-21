using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void ��ġ��_�������_����()
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

    // ������ �˾Ƽ� �����
}
