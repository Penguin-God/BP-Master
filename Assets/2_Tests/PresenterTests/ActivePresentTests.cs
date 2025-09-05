using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class ActivePresentTests
{
    private IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers;
    ActiveTargetPresenter CreateSut() => new ActiveTargetPresenter(Team.Blue, teamMembers);
    [SetUp]
    public void SetUp()
    {
        teamMembers = new Dictionary<Team, IReadOnlyList<int>>
        {
            { Team.Blue, new List<int> { 1, 2, 3 } },
            { Team.Red,  new List<int> { 10, 11, 12 } }
        };
    }

    [Test]
    public void 잘못된_대상은_null_반환()
    {
        var sut = CreateSut();
        sut.SelectTrait(Side.Opponent, TraitTargetType.Single);

        Assert.IsNull(sut.GetTargets(42));
        Assert.IsNull(sut.GetTargets(1));
    }

    [Test]
    public void 싱글은_단일_대상_반환()
    {
        var sut = CreateSut();
        sut.SelectTrait(Side.Opponent, TraitTargetType.Single);

        CollectionAssert.AreEqual(new int[] { 10 }, sut.GetTargets(10));
    }

    [Test]
    public void All은_전체_반환()
    {
        var sut = CreateSut();
        sut.SelectTrait(Side.Opponent, TraitTargetType.All);

        CollectionAssert.AreEqual(new int[] { 10, 11, 12 }, sut.GetTargets(10));
    }
}
