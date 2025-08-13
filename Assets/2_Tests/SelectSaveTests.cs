using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
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
        var info = new SelectInfo(team, select, 3);
        GameBanPickStorage storage = new();
        storage.SaveSelect(info);

        Assert.AreEqual(3, storage.GetStorage(team, select)[0]);
        Assert.AreEqual(1, storage.GetStorage(team, select).Count);
    }
}
