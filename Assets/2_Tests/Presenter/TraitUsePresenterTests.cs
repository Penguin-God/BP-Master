using NUnit.Framework;
using System.Collections.Generic;

public class TraitUsePresenterTests
{
    TraitUsePresenter presenter;

    [SetUp]
    public void SetUp()
    {
        var blueChamp = new Champion(0, "", default, new Trait(Side.Self, TargetRange.Single, new TestAttackChanger(0)));
        var redChamp = new Champion(0, "", default, new Trait(Side.Self, TargetRange.Single, new TestAttackChanger(0)));

        var teams = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new List<Champion>{ blueChamp } },
            { Team.Red, new List<Champion>{ redChamp } }
        };

        var phaseManager = new PhaseManager(new[] { new PhaseData(GamePhase.Trait, new Phase(new[] { Team.Blue, Team.Red })) });
        presenter = new TraitUsePresenter(new TraitController(teams), phaseManager);

        phaseManager.Start();
    }

    [Test]
    public void 결과에_따라_enum반환()
    {
        Assert.AreEqual(TraitClickResult.Faild, presenter.ClickChampion(CreateSlot(Team.Red, 0)));

        Assert.AreEqual(TraitClickResult.Select, presenter.ClickChampion(CreateSlot(Team.Blue, 0)));

        Assert.AreEqual(TraitClickResult.Use, presenter.ClickChampion(CreateSlot(Team.Blue, 0)));
    }

    [Test]
    public void 특성_사용_후_이벤트()
    {
        Team result = Team.Red;
        presenter.OnTraitUsed += _ => result = _;

        presenter.ClickChampion(CreateSlot(Team.Blue, 0)); // 선택
        presenter.ClickChampion(CreateSlot(Team.Red, 0));  // 사용

        Assert.AreEqual(Team.Blue, result);
    }

    ChampionSlot CreateSlot(Team team, int index) => new ChampionSlot(team, index);
}