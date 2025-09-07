using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class TraitTargetFindingTests
{
    IReadOnlyDictionary<Team, IReadOnlyList<int>> teamMembers;
    [SetUp]
    public void SetUp()
    {
        teamMembers = new Dictionary<Team, IReadOnlyList<int>>
        {
            { Team.Blue, new List<int> { 1, 2 } },
            { Team.Red,  new List<int> { 11, 12 } }
        };
    }

    [Test]
    public void 싱글은_단일_대상_반환()
    {
        var sut = new TraitTargetFinder(teamMembers);

        Assert.AreEqual(2, sut.GetTargets(Team.Blue, TargetRange.Single, 1).First());
        Assert.AreEqual(11, sut.GetTargets(Team.Red, TargetRange.Single, 0).First());
    }

    [Test]
    public void All은_전체_반환()
    {
        var sut = new TraitTargetFinder(teamMembers);

        CollectionAssert.AreEqual(new int[] { 1, 2 }, sut.GetTargets(Team.Blue, TargetRange.All, 1));
        CollectionAssert.AreEqual(new int[] { 11, 12 }, sut.GetTargets(Team.Red, TargetRange.All, 0));
    }
}
