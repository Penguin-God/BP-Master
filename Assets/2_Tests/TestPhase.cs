using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestPhase
{
    [Test]
    public void 순서에_맞게_진행()
    {
        GamePhase[] sequence = new GamePhase[] { GamePhase.Ban, GamePhase.Pick, GamePhase.Pick, GamePhase.Swap };
        PhaseManager sut = new PhaseManager(sequence);

        Assert.AreEqual(sut.NextPhase(), GamePhase.Pick);
        Assert.AreEqual(sut.NextPhase(), GamePhase.Pick);
        Assert.AreEqual(sut.NextPhase(), GamePhase.Swap);
    }
}
