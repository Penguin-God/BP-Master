using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AI_ActionTests
{
    [Test]
    public void AI는_주어진_풀_안에서만_선택()
    {
        GameBanPickStorage gameBanPickStorage = new GameBanPickStorage();

        IEnumerable<Champion> champions = new Champion[] { CreateChampion(1), CreateChampion(2), CreateChampion(3) };
        IEnumerable<int> ids = champions.Select(x => x.Id);
        RandomSelector sut = new();

        int result = sut.Ban(champions);

        CollectionAssert.Contains(ids, result);
    }

    [Test]
    public void 랜덤_선택()
    {
        IEnumerable<Champion> champions = new Champion[] { CreateChampion(1), CreateChampion(2), CreateChampion(3) };
        IEnumerable<int> ids = champions.Select(x => x.Id);
        RandomSelector sut = new();

        int result = sut.Ban(champions);

        CollectionAssert.Contains(ids, result);
    }

    Champion CreateChampion(int id) => new Champion(id, "", default);
}
