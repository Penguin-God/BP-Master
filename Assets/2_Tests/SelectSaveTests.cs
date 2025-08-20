using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TestTools;

public class SelectSaveTests
{
    [Test]
    [TestCase(Team.Red, SelectType.Ban)]
    [TestCase(Team.Blue, SelectType.Ban)]
    [TestCase(Team.Red, SelectType.Pick)]
    [TestCase(Team.Blue, SelectType.Pick)]
    public void Test_Save(Team team, SelectType select)
    {
        const int id = 3;
        var info = new SelectInfo(team, select, id);
        GameBanPickStorage storage = CreateStorage(id);
        storage.SaveSelect(info);

        Assert.AreEqual(id, storage.GetStorage(team, select)[0]);
        Assert.AreEqual(1, storage.GetStorage(team, select).Count);
    }

    [Test]
    public void 선택_가능한_챔들_반환()
    {
        GameBanPickStorage sut = CreateStorage(1, 2, 3);

        sut.SaveSelect(new SelectInfo(Team.Blue, SelectType.Ban, 1));

        CollectionAssert.DoesNotContain(sut.SelectableIds, 1);
    }

    [Test]
    public void 중복_선택_불가()
    {
        const int Id = 3;
        GameBanPickStorage storage = CreateStorage(Id);

        Assert.IsTrue(storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Ban, Id)));
        Assert.IsFalse(storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Ban, Id)));
        Assert.IsFalse(storage.SaveSelect(new SelectInfo(Team.Red, SelectType.Pick, Id)));
    }

    [Test]
    [TestCase(Team.Red, new[] { 3, 7 })]
    [TestCase(Team.Blue, new[] { 4, 9 })]
    public void 단일팀_연속선택_순서_및_내용_보장(Team team, int[] pickIds)
    {
        var storage = CreateStorage(pickIds);

        foreach (var id in pickIds)
            storage.SaveSelect(new SelectInfo(team, SelectType.Pick, id));

        var list = storage.GetStorage(team, SelectType.Pick);
        Assert.AreEqual(pickIds.Length, list.Count, "저장 개수가 일치해야 합니다.");
        CollectionAssert.AreEqual(pickIds, list, "순서 및 내용이 일치해야 합니다.");
    }


    [Test]
    public void 교차_선택_시_팀별_저장_보장()
    {
        var storage = CreateStorage(101, 102, 201, 202);

        storage.SaveSelect(new SelectInfo(Team.Red, SelectType.Pick, 101));
        storage.SaveSelect(new SelectInfo(Team.Red, SelectType.Pick, 102));
        storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Pick, 201));
        storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Pick, 202));

        var redList = storage.GetStorage(Team.Red, SelectType.Pick);
        var blueList = storage.GetStorage(Team.Blue, SelectType.Pick);

        CollectionAssert.AreEqual(new[] { 101, 102 }, redList);
        CollectionAssert.AreEqual(new[] { 201, 202 }, blueList);

        Assert.IsFalse(blueList.Contains(101));
        Assert.IsFalse(blueList.Contains(102));
        Assert.IsFalse(redList.Contains(201));
        Assert.IsFalse(redList.Contains(202));

        Assert.AreEqual(2, redList.Count);
        Assert.AreEqual(2, blueList.Count);
    }


    [Test]
    public void Ban과Pick_순서_섞여도_간섭없음()
    {
        var storage = CreateStorage(11, 22, 101, 102, 201);

        storage.SaveSelect(new SelectInfo(Team.Red, SelectType.Ban, 11));
        storage.SaveSelect(new SelectInfo(Team.Red, SelectType.Pick, 101));
        storage.SaveSelect(new SelectInfo(Team.Red, SelectType.Pick, 102));
        storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Ban, 22));
        storage.SaveSelect(new SelectInfo(Team.Blue, SelectType.Pick, 201));

        CollectionAssert.AreEqual(new[] { 11 }, storage.GetStorage(Team.Red, SelectType.Ban));
        CollectionAssert.AreEqual(new[] { 22 }, storage.GetStorage(Team.Blue, SelectType.Ban));
        CollectionAssert.AreEqual(new[] { 101, 102 }, storage.GetStorage(Team.Red, SelectType.Pick));
        CollectionAssert.AreEqual(new[] { 201 }, storage.GetStorage(Team.Blue, SelectType.Pick));
    }

    GameBanPickStorage CreateStorage(params int[] selectableIds) => new GameBanPickStorage(selectableIds);
}
