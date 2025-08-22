using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void 매치는_순서대로_진행()
    {
        AgentManager agentManager = new(new GameBanPickStorage(new int[] { 0, 1, 2, 3 }));
        
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        MatchManager sut = new(phaseManager, agentManager);

        sut.GameStart();
        Assert.AreEqual(Team.Blue, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Ban, sut.CurrentPhase);

        agentManager.Ban(Team.Blue, 0);

        Assert.AreEqual(Team.Red, sut.CurrentTurn);

        agentManager.Ban(Team.Red, 1);

        Assert.AreEqual(Team.Blue, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Pick, sut.CurrentPhase);

        agentManager.Pick(Team.Blue, 2);
        agentManager.Pick(Team.Red, 3);

        Assert.AreEqual(Team.All, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Swap, sut.CurrentPhase);

        agentManager.SwapDone(Team.Blue);

        Assert.AreEqual(GamePhase.Swap, sut.CurrentPhase);

        agentManager.SwapDone(Team.Red);

        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }
}
