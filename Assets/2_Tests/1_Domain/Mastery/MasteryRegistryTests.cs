using NUnit.Framework;
using System.Collections.Generic;

public class MasteryRegistryTests
{
    [Test]
    public void 생성자는_팀별_마스터리_설정()
    {
        var blueMastery = new MasteryStatCollection(new ChampionMastery[0]);
        var redMastery = new MasteryStatCollection(new ChampionMastery[0]);

        var registry = new MasteryRegistry(blueMastery, redMastery);

        Assert.AreEqual(blueMastery, registry.GetTeamMasteryCollection(Team.Blue));
        Assert.AreEqual(redMastery, registry.GetTeamMasteryCollection(Team.Red));
    }

    [Test]
    [TestCase(Team.Blue)]
    [TestCase(Team.Red)]
    public void 진영별_마스터리_설정_및_조회가_정확히_동작해야_함(Team team)
    {
        var sut = CreateSut();
        var expected = CreateEmptyCollection();

        sut.InitTeamMastery(team, expected);
        var result = sut.GetTeamMasteryCollection(team);

        Assert.AreSame(expected, result);
    }

    [Test]
    public void 데이터가_없는_진영_조회_시_null을_반환해야_함()
    {
        var sut = CreateSut();

        var result = sut.GetTeamMasteryCollection(Team.Blue);

        Assert.IsNull(result);
    }

    MasteryRegistry CreateSut() => new MasteryRegistry();

    MasteryStatCollection CreateEmptyCollection() => new MasteryStatCollection(new List<ChampionMastery>());
}