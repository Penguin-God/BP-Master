using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void 매치는_자동_진행()
    {
        var storage = new GameBanPickStorage(new int[] { 0, 1, 2, 3 });
        DraftActionController draftController = new(storage);

        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        MatchManager sut = new(phaseManager, draftController, new TestActionHandler(Team.Blue, storage), new TestActionHandler(Team.Red, storage));

        sut.GameStart();
        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }
}