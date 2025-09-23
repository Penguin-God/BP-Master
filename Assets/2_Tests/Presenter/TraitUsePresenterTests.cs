using NUnit.Framework;
using static TestHelper;

public class TraitUsePresenterTests
{
    TraitUsePresenter presenter;

    [SetUp]
    public void SetUp()
    {
        SlotStorage<Champion> picks = new();
        picks.AddSlot(Team.Blue, CreateTraitChamp(Side.Opponent, TargetRange.All, 0));
        picks.AddSlot(Team.Blue, CreateTraitChamp(Side.Opponent, TargetRange.All, 0));
        picks.AddSlot(Team.Blue, CreateTraitChamp(Side.Opponent, TargetRange.All, 0));

        picks.AddSlot(Team.Red, CreateTraitChamp(Side.Self, TargetRange.Single, 0));
        picks.AddSlot(Team.Red, CreateTraitChamp(Side.Self, TargetRange.Single, 0));
        picks.AddSlot(Team.Red, CreateTraitChamp(Side.Self, TargetRange.Single, 0));

        SlotStorage<ChampionStatus> statuses= new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());

        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());

        presenter = new TraitUsePresenter(new TraitController(statuses), picks);
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

        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), presenter.GetClickableSlots());
        presenter.ClickChampion(CreateSlot(Team.Blue, 1));
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2), presenter.GetClickableSlots());
        presenter.ClickChampion(CreateSlot(Team.Red, 0));

        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 2), presenter.GetClickableSlots());
    }

    SlotData CreateSlot(Team team, int index) => new SlotData(team, index);
}