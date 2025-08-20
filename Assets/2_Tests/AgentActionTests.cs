using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AgentActionTests
{
    [Test]
    public void 에이전트_밴_알림()
    {
        bool isDone = false;
        TeamBanPickStorage storage = new();
        var agent = new ActionAgent(storage);
        agent.OnActionDone += () => isDone = true;

        agent.Ban(1);

        Assert.AreEqual(1, storage.GetStorage(SelectType.Ban).Count);
        Assert.AreEqual(1, storage.GetStorage(SelectType.Ban)[0]);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void 에이전트_픽_알림()
    {
        bool isDone = false;
        TeamBanPickStorage storage = new();
        var agent = new ActionAgent(storage);
        agent.OnActionDone += () => isDone = true;

        agent.Pick(1);

        Assert.AreEqual(1, storage.GetStorage(SelectType.Pick).Count);
        Assert.AreEqual(1, storage.GetStorage(SelectType.Pick)[0]);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void 에이전트_스왑_알림()
    {
        bool isDone = false;
        TeamBanPickStorage storage = new();
        storage.SaveSelect(SelectType.Pick, 1);
        storage.SaveSelect(SelectType.Pick, 2);
        var agent = new ActionAgent(storage);
        agent.OnActionDone += () => isDone = true;

        agent.Swap(0, 1);
        Assert.IsFalse(isDone);
        Assert.AreEqual(2, storage.GetStorage(SelectType.Pick)[0]);
        Assert.AreEqual(1, storage.GetStorage(SelectType.Pick)[1]);

        agent.SwapDone();
        Assert.IsTrue(isDone);
    }
}
