using NUnit.Framework;
using System.Collections.Generic;

public class TraitUsePresenterTests
{
    TraitUsePresenter presenter;

    [SetUp]
    public void SetUp()
    {
        var blueChamp = new Champion(0, "", default, new Trait(Side.Opponent, TargetRange.All, new TestAttackChanger(0)));
        var redChamp = new Champion(0, "", default, new Trait(Side.Self, TargetRange.Single, new TestAttackChanger(0)));

        var teams = new Dictionary<Team, IReadOnlyList<Champion>>
        {
            { Team.Blue, new List<Champion>{ blueChamp, blueChamp, blueChamp } },
            { Team.Red, new List<Champion>{ redChamp, redChamp, redChamp } }
        }; // 만들기 귀찮아서 복붙했는데 이럼 특성이 싹다 적용되긴해
        presenter = new TraitUsePresenter(new TraitController(teams));
    }

    [Test]
    public void 결과에_따라_enum반환()
    {
        presenter.ChangeTeam(Team.Blue);
        Assert.AreEqual(TraitClickResult.Faild, presenter.ClickChampion(CreateSlot(Team.Red, 0)));

        Assert.AreEqual(TraitClickResult.Select, presenter.ClickChampion(CreateSlot(Team.Blue, 0)));

        Assert.AreEqual(TraitClickResult.Use, presenter.ClickChampion(CreateSlot(Team.Blue, 0)));
    }

    [Test]
    public void 특성_사용_후_이벤트()
    {
        Team result = Team.Red;
        presenter.OnTraitUsed += _ => result = _;
        presenter.ChangeTeam(Team.Blue);

        presenter.ClickChampion(CreateSlot(Team.Blue, 0)); // 선택
        presenter.ClickChampion(CreateSlot(Team.Red, 0));  // 사용

        Assert.AreEqual(Team.Blue, result);
    }

    [Test]
    public void 선택_가능_슬롯들_반환()
    {
        presenter.ChangeTeam(Team.Blue);

        CollectionAssert.AreEquivalent(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 1), CreateSlot(Team.Blue, 2)), presenter.GetClickableSlots());
        presenter.ClickChampion(CreateSlot(Team.Blue, 1));
        UnityEngine.Debug.Log(presenter.GetClickableSlots());
        CollectionAssert.AreEquivalent(CreateSlots(CreateSlot(Team.Red, 0), CreateSlot(Team.Red, 1), CreateSlot(Team.Red, 2)), presenter.GetClickableSlots());
        presenter.ClickChampion(CreateSlot(Team.Red, 0));

        CollectionAssert.AreEquivalent(CreateSlots(CreateSlot(Team.Blue, 0), CreateSlot(Team.Blue, 2)), presenter.GetClickableSlots());
    }

    ChampionSlot[] CreateSlots(params ChampionSlot[] slots) => slots;
    ChampionSlot CreateSlot(Team team, int index) => new ChampionSlot(team, index);
}