using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnumCastingTests
{
    [Test]
    public void 타겟_팀_반환()
    {
        Assert.AreEqual(Team.Blue, TeamSideConverter.GetTargetTeam(Team.Blue, Side.Self));
        Assert.AreEqual(Team.Red, TeamSideConverter.GetTargetTeam(Team.Blue, Side.Opponent));
    }
}
