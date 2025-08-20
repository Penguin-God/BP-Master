using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameProgressTests
{
    [Test]
    public void Agent��_�ൿ_�Ϸ�_��_�˸�()
    {
        bool result = false;
        var agent = new ActionAgent(new TeamBanPickStorage());
        agent.OnActionDone += () => result = true;

        agent.OnRequestAction(GamePhase.Pick);
        agent.Done();
        Assert.IsTrue(result);
    }

    [Test]
    public void Agent��_��û��_����_�ൿ_����()
    {
        TeamBanPickStorage storage = new();
        var agent = new ActionAgent(storage);

        agent.OnRequestAction(GamePhase.Ban);
        agent.Done();
        Assert.AreEqual(1, storage.GetStorage(SelectType.Ban).Count);

        agent.OnRequestAction(GamePhase.Pick);
        agent.Done();
        Assert.AreEqual(1, storage.GetStorage(SelectType.Pick).Count);
    }

    [Test]
    public void Agent��_��û��_����_�ൿ_����_��_�˸�()
    {
        bool result = false;
        TeamBanPickStorage storage = new();
        var agent = new ActionAgent(storage);
        agent.OnActionDone += () => result = true;

        agent.OnRequestAction(GamePhase.Ban);

        Assert.AreEqual(0, storage.GetStorage(SelectType.Ban).Count);
        Assert.IsFalse(result);

        agent.Done();
        Assert.AreEqual(1, storage.GetStorage(SelectType.Ban).Count);
        Assert.IsTrue(result);
    }
}
