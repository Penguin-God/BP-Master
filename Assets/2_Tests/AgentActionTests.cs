using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AgentActionTests
{
    [Test]
    public void ������Ʈ_������_��û��_��_����_�޾ƾ�_�Ѿ()
    {
        bool isDone = false;
        AgentManager sut = new(new ActionAgent(new()), new ActionAgent(new()), new GameBanPickStorage(new int[] { 1, 2, 3 }));
        sut.OnActionDone += () => isDone = true;
        sut.PhaseChange(GamePhase.Swap);

        sut.SwapDone(Team.Red);
        Assert.IsFalse(isDone);
        sut.SwapDone(Team.Blue);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void ������Ʈ_��_�˸�()
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
    public void ������Ʈ_��_�˸�()
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
    public void ������Ʈ_����_�˸�()
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
