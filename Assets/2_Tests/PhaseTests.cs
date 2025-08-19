using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PhaseTests
{
    [Test]
    public void ��_���࿡_����_����_����()
    {
        Team[] teams = new Team[] { Team.Red, Team.Blue };
        Phase phase = new(GamePhase.Ban, new Queue<Team>(teams));

        Assert.AreEqual(Team.Red, phase.CurrentTeam);
        Assert.IsTrue(phase.Next());
        Assert.IsFalse(phase.IsDone);

        Assert.AreEqual(Team.Blue, phase.CurrentTeam);
        Assert.IsFalse(phase.Next());
        Assert.IsTrue(phase.IsDone);
    }
}
