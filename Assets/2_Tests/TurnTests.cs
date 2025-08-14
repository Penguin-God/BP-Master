using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TurnTests
{
    [Test]
    public void 순서에_맞게_진행()
    {
        Team[] sequence = new Team[] { Team.Blue, Team.Red, Team.Red, Team.Blue };
        TurnManager sut = new TurnManager(sequence);

        Assert.AreEqual(sut.NextTurn(), Team.Red);
        Assert.AreEqual(sut.NextTurn(), Team.Red);
        Assert.AreEqual(sut.NextTurn(), Team.Blue);
    }
}
