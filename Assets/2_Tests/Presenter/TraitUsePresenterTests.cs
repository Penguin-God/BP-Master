using NUnit.Framework;
using System.Collections.Generic;

public class TraitUsePresenterTests
{
    TraitUsePresenter presenter;

    [SetUp]
    public void SetUp()
    {
        var blueChamp = TestHelper.CreateTraitChamp(Side.Opponent, TargetRange.All, 0);
        var redChamp = TestHelper.CreateTraitChamp(Side.Self, TargetRange.Single, 0);

        var teams = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new List<Champion>{ blueChamp, blueChamp, blueChamp } },
            { Team.Red, new List<Champion>{ redChamp, redChamp, redChamp } }
        }; // 만들기 귀찮아서 복붙했는데 이럼 특성이 싹다 적용되긴해
        presenter = new TraitUsePresenter(new TraitController(teams));
    }

    [Test]
    public void 클릭_결과에_따라_enum반환()
    {
        presenter.ChangeTeam(Team.Blue);
        Assert.AreEqual(TraitClickResult.Faild, presenter.ClickChampion(CreateSlot(Team.Red, 0)));

        Assert.AreEqual(TraitClickResult.Select, presenter.ClickChampion(CreateSlot(Team.Blue, 0)));

        Assert.AreEqual(TraitClickResult.Use, presenter.ClickChampion(CreateSlot(Team.Red, 0)));
    }

    [Test]
    public void 특성_사용_후_이벤트_호출()
    {
        Team result = Team.Red;
        presenter.OnTraitUsed += _ => result = _;
        presenter.ChangeTeam(Team.Blue);

        presenter.ClickChampion(CreateSlot(Team.Blue, 0)); // 선택
        presenter.ClickChampion(CreateSlot(Team.Red, 0));  // 사용

        Assert.AreEqual(Team.Blue, result);
    }

    [Test]
    public void 클릭_상태에_따라_선택_가능한_슬롯들_반환()
    {
        presenter.ChangeTeam(Team.Blue);

        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(0, 1, 2), presenter.GetClickableSlots());
        presenter.ClickChampion(CreateSlot(Team.Blue, 1));
        CollectionAssert.AreEquivalent(TestHelper.CreateRedSlots(0, 1, 2), presenter.GetClickableSlots());
        presenter.ClickChampion(CreateSlot(Team.Red, 0));

        CollectionAssert.AreEquivalent(TestHelper.CreateBlueSlots(0, 2), presenter.GetClickableSlots());
    }

    ChampionSlot CreateSlot(Team team, int index) => new ChampionSlot(team, index);
}