using NUnit.Framework;
using System;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

public class AgentActionTests
{
    [Test]
    public void ����_��_�ƴ�_���_����()
    {
        bool isDone = false;
        var storage = new GameBanPickStorage(new int[] { 1, 2, 3 });
        AgentManager sut = new(new ActionAgent(new()), new ActionAgent(new()), storage);
        sut.OnActionDone += () => isDone = true;

        sut.PhaseChange(GamePhase.Ban);
        sut.Pick(Team.Blue, 1);
        Assert.IsFalse(isDone);

        sut.PhaseChange(GamePhase.Pick);
        sut.Ban(Team.Blue, 1);
        Assert.IsFalse(isDone);
    }

    [Test]
    public void ��_����_��_�˸�()
    {
        bool isDone = false;
        var storage = new GameBanPickStorage(new int[] { 1, 2, 3 });
        AgentManager sut = new(new ActionAgent(new()), new ActionAgent(new()), storage);
        sut.OnActionDone += () => isDone = true;
        sut.PhaseChange(GamePhase.Pick);

        sut.Pick(Team.Blue, 1);

        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick).Count);
        Assert.AreEqual(1, storage.GetStorage(Team.Blue, SelectType.Pick)[0]);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void ��_����_��_�˸�()
    {
        bool isDone = false;
        var storage = new GameBanPickStorage(new int[] { 1, 2, 3 });
        AgentManager sut = new(new ActionAgent(new()), new ActionAgent(new()), storage);
        sut.OnActionDone += () => isDone = true;
        sut.PhaseChange(GamePhase.Ban);

        sut.Ban(Team.Red, 1);

        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban).Count);
        Assert.AreEqual(1, storage.GetStorage(Team.Red, SelectType.Ban)[0]);
        Assert.IsTrue(isDone);
    }

    [Test]
    public void ������_��û��_��_����_�޾ƾ�_�˸�()
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
