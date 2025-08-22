using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class PhaseTests
{
    [Test]
    public void 다음턴_반환()
    {
        Team[] teams = new Team[] { Team.Red, Team.Blue };
        Phase phase = new(teams);

        Assert.AreEqual(Team.Red, phase.GetNext());
        Assert.AreEqual(Team.Blue, phase.GetNext());
    }

    [Test]
    public void 턴_없는데_달라_하면_에러()
    {
        Phase phase = new(new Team[] { Team.Red });

        phase.GetNext();
        Assert.Throws<InvalidOperationException>(() => phase.GetNext());
    }

    [Test]
    public void 턴_진행에_따라_페이즈_갱신()
    {
        PhaseData[] phases = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Red, Team.Blue })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Red, Team.Blue })),
        };

        PhaseManager sut = new(phases);
        sut.GameStart();

        Assert.IsTrue(sut.Next());
        Assert.AreEqual(GamePhase.Ban, sut.CurrentPhase);
        Assert.IsTrue(sut.Next());

        Assert.AreEqual(GamePhase.Pick, sut.CurrentPhase);

        Assert.IsTrue(sut.Next());
        Assert.IsTrue(sut.Next());

        Assert.IsFalse(sut.Next());
    }
}
