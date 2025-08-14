using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FlowTests
{
    [Test]
    public void ����_������_����_����()
    {
        GameFlowManager flowManager = new GameFlowManager(new GamePhase[] { }, new Team[] { Team.Red, Team.Blue }, new Team[] { Team.Red, Team.Blue });

        flowManager.Ban(1);
        flowManager.Ban(2);
        Assert.AreEqual(GamePhase.Pick, flowManager.CurrentGamePhase);

        flowManager.Pick(4);
        flowManager.Pick(4);
        Assert.AreEqual(GamePhase.Swap, flowManager.CurrentGamePhase);
    }

    [Test]
    public void è�Ǿ�_Ǯ_�ȿ�����_����_����()
    {

    }
}
