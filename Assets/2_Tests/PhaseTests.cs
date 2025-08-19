using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PhaseTests
{
    [Test]
    public void ��_���࿡_����_����_��_����_����()
    {
        Team[] teams = new Team[] { Team.Red, Team.Blue };
        Phase phase = new (teams);

        Assert.AreEqual(Team.Red, phase.CurrentTeam);
        Assert.IsTrue(phase.Next());
        Assert.IsFalse(phase.IsDone);

        Assert.AreEqual(Team.Blue, phase.CurrentTeam);
        Assert.IsTrue(phase.Next());
        Assert.IsTrue(phase.IsDone);

        Assert.IsFalse(phase.Next());
    }

    [Test]
    public void ��_���࿡_����_������_����()
    {
        PhaseData[] phases = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Red, Team.Blue })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Red, Team.Blue })),
        };

        PhaseManager sut = new(phases);


        Assert.IsTrue(sut.Next());
        Assert.AreEqual(GamePhase.Ban, sut.CurrentPhaseData.GamePhase);
        Assert.IsTrue(sut.Next());

        Assert.AreEqual(GamePhase.Pick, sut.CurrentPhaseData.GamePhase);

        Assert.IsTrue(sut.Next());
        Assert.IsTrue(sut.Next());

        Assert.IsFalse(sut.Next());
    }
}
