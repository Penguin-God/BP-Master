using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class ActivePresentTests
{
    private IReadOnlyDictionary<Team, IReadOnlyList<Champion>> teamMembers;
    ActiveTargetPresenter CreateTargetPersenter() => new ActiveTargetPresenter(Team.Blue, teamMembers);
    [SetUp]
    public void SetUp()
    {
        teamMembers = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new List<Champion> { CreateChampion(1, Side.Opponent, TargetRange.Single), CreateChampion(2, Side.Opponent, TargetRange.Single) } },
            { Team.Red,  new List<Champion> { CreateChampion(11, Side.Opponent, TargetRange.Single), CreateChampion(12, Side.Opponent, TargetRange.Single) } }
        };
    }

    Champion CreateChampion(int id, Side targetSide, TargetRange range) => new Champion(id, "", default, new Trait(targetSide, range, null));

    [Test]
    public void 선택과_취소()
    {
        var sut = CreateTargetPersenter();

        sut.SelectChamp(1);
        CollectionAssert.AreEqual(new int[] { 11 }, sut.GetTargetIds(11));

        sut.Cancle();
        Assert.IsNull(sut.GetTargetIds(11));
    }

    //[Test]
    //public void 잘못된_대상은_null_반환()
    //{
    //    var sut = CreateTargetPersenter();

    //    sut.SelectChamp(1);

    //    Assert.IsNull(sut.GetTargetIds(42));
    //    Assert.IsNull(sut.GetTargetIds(1));
    //}

    //[Test]
    //public void 싱글은_단일_대상_반환()
    //{
    //    var sut = CreateTargetPersenter();

    //    CollectionAssert.AreEqual(new int[] { 10 }, sut.GetTargets(10, Side.Opponent, TargetRange.Single));
    //    CollectionAssert.AreEqual(new int[] { 2 }, sut.GetTargets(2, Side.Self, TargetRange.Single));
    //}

    //[Test]
    //public void All은_전체_반환()
    //{
    //    var sut = CreateTargetPersenter();
        
    //    CollectionAssert.AreEqual(new int[] { 10, 11, 12 }, sut.GetTargets(10, Side.Opponent, TargetRange.All));
    //    CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, sut.GetTargets(2, Side.Self, TargetRange.All));
    //}
}
