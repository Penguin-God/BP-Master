using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MatchTests
{
    [Test]
    public void ��ġ��_�������_����()
    {
        ActionAgent blue = new ActionAgent(new ());
        ActionAgent red = new ActionAgent(new ());
        
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        MatchManager sut = new(phaseManager, blue, red); // �ù� �ߺ� ���� ��� �ϳ�?

        sut.GameStart();

        Assert.AreEqual(Team.Blue, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Ban, sut.CurrentPhase);

        blue.Ban(0);
        Assert.AreEqual(Team.Red, sut.CurrentTurn);

        red.Ban(1);
        Assert.AreEqual(Team.Blue, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Pick, sut.CurrentPhase);

        blue.Pick(2);
        red.Pick(3);

        Assert.AreEqual(Team.All, sut.CurrentTurn);
        Assert.AreEqual(GamePhase.Swap, sut.CurrentPhase);

        blue.SwapDone(); // swapdone�� ����
        Assert.AreEqual(GamePhase.Swap, sut.CurrentPhase);

        red.SwapDone();
        Assert.AreEqual(GamePhase.Done, sut.CurrentPhase);
    }
}
