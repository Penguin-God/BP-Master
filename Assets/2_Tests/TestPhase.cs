using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPhase
{
    [Test]
    public void ������_�°�_����()
    {
        GamePhase[] sequence = new GamePhase[] { GamePhase.Ban, GamePhase.Pick, GamePhase.Pick, GamePhase.Swap };
        PhaseManager sut = new PhaseManager(sequence);

        Assert.AreEqual(sut.NextPhase(), GamePhase.Pick);
        Assert.AreEqual(sut.NextPhase(), GamePhase.Pick);
        Assert.AreEqual(sut.NextPhase(), GamePhase.Swap);
    }
}
