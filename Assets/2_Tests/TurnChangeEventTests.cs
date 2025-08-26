using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TurnChangeEventTests
{
    [Test]
    public void 올바른_페이즈와_팀이면_true와_알림()
    {
        int count = 0;
        ActionEventBus sut = new();
        sut.ChangeTeam(Team.Blue);
        sut.OnChangeTurn += () => count++;

        bool result = sut.ActionDone(Team.Blue);

        Assert.IsTrue(result);
        Assert.AreEqual(1, count);
    }

    [Test]
    public void 잘못된_팀이면_false와_알림없음()
    {
        int count = 0;
        ActionEventBus sut = new();
        sut.ChangeTeam(Team.Blue);
        sut.OnChangeTurn += () => count++;

        bool result = sut.ActionDone(Team.Red);

        Assert.IsFalse(result);
        Assert.AreEqual(0, count);
    }

    [Test]
    public void All은_요청을_각_팀에_받아야_알림()
    {
        int count = 0;
        ActionEventBus sut = new();
        sut.ChangeTeam(Team.All);
        sut.OnChangeTurn += () => count++;

        Assert.IsFalse(sut.ActionDone(Team.Blue));
        Assert.IsTrue(sut.ActionDone(Team.Red));
        Assert.AreEqual(1, count);
    }
}
