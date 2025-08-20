using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AI_ActionTests
{
    [Test]
    public void AI��_�־���_Ǯ_�ȿ�����_����()
    {
        GameBanPickStorage gameBanPickStorage = new GameBanPickStorage();

        IEnumerable<Champion> champions = new Champion[] { CreateChampion(1), CreateChampion(2), CreateChampion(3) };
        IEnumerable<int> ids = champions.Select(x => x.Id);
        RandomSelector sut = new();

        int result = sut.Ban(champions);

        CollectionAssert.Contains(ids, result);
    }

    [Test]
    public void ����_����()
    {
        IEnumerable<Champion> champions = new Champion[] { CreateChampion(1), CreateChampion(2), CreateChampion(3) };
        IEnumerable<int> ids = champions.Select(x => x.Id);
        RandomSelector sut = new();

        int result = sut.Ban(champions);

        CollectionAssert.Contains(ids, result);
    }

    [Test]
    public void Agnet��_�����ڸ�_������()
    {
        int count = 0;
        ActionAgent agent = new ActionAgent(new TeamBanPickStorage());
        agent.OnActionDone += () => count++;
        AI_Agent sut = new(agent);
        sut.ActoinRequset(GamePhase.Ban);

        Assert.AreEqual(1, count);
    }

    Champion CreateChampion(int id) => new Champion(id, "", default);
}
