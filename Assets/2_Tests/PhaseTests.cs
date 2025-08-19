using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PhaseTests
{
    [Test]
    public void 턴_진행에_따라_상태_갱신()
    {
        Team[] teams = new Team[] { Team.Red, Team.Blue };
        Phase phase = new Phase(GamePhase.Ban, new Queue<Team>(teams));

        Assert.AreEqual(Team.Red, phase.CurrentTeam);
        Assert.IsTrue(phase.Next());
        Assert.IsFalse(phase.IsDone);

        Assert.AreEqual(Team.Blue, phase.CurrentTeam);
        Assert.IsTrue(phase.Next());
        Assert.IsTrue(phase.IsDone);

        Assert.IsFalse(phase.Next());
    }
}
