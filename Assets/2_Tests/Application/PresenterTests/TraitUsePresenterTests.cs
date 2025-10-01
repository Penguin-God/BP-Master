using NUnit.Framework;
using static TestHelper;

public class TraitUsePresenterTests
{
    TraitUsePresenter presenter;
    SlotStorage<ChampionStatus> statuses;

    [SetUp]
    public void SetUp()
    {
        SlotStorage<Champion> picks = new();
        picks.AddSlot(Team.Blue, CreateTraitChamp(Side.Opponent, TargetRange.All, 10));
        picks.AddSlot(Team.Blue, CreateTraitChamp(Side.Opponent, TargetRange.All, 10));
        picks.AddSlot(Team.Blue, CreateTraitChamp(Side.Opponent, TargetRange.All, 10));

        picks.AddSlot(Team.Red, CreateTraitChamp(Side.Self, TargetRange.Single, 10));
        picks.AddSlot(Team.Red, CreateTraitChamp(Side.Self, TargetRange.Single, 10));
        picks.AddSlot(Team.Red, CreateTraitChamp(Side.Self, TargetRange.Single, 10));

        statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());

        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());

        presenter = new TraitUsePresenter(new TraitUseFacade(statuses), picks);
    }

    [Test]
    public void 특성_사용_적용()
    {
        presenter.ChangeTeam(Team.Blue);

        presenter.ClickChampion(CreateSlot(Team.Blue, 0)); // 선택
        presenter.ClickChampion(CreateSlot(Team.Red, 0));  // 사용

        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }

    [Test]
    public void 클릭_상태에_따라_선택_가능한_슬롯들_반환()
    {
        presenter.ChangeTeam(Team.Blue);
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), presenter.GetClickableSlots());

        presenter.ClickChampion(CreateSlot(Team.Blue, 1));
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2), presenter.GetClickableSlots());

        // blue 1번째 사용했으니 그건 제외
        presenter.ClickChampion(CreateSlot(Team.Red, 0));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 2), presenter.GetClickableSlots());
    }

    [Test]
    public void 특성_사용_가능한_슬롯들_필터링()
    {
        TraitSlotFilter sut = new (3, new TraitUseFacade(statuses));
        statuses.GetSlot(CreateBlueSlot(1)).UseTrait();

        var result = sut.FilteringUseableSlots(Team.Blue);

        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 2), result);
    }

    [Test]
    public void 특성_타겟_슬롯들_필터링()
    {
        TraitSlotFilter sut = new(2, new TraitUseFacade(statuses));

        var result = sut.FilteringTargetSlots(Team.Blue, new Side[] { Side.Opponent });
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1), result);

        result = sut.FilteringTargetSlots(Team.Blue, new Side[] { Side.Opponent, Side.Self });
        CollectionAssert.AreEquivalent(new SlotData[] { CreateBlueSlot(0), CreateBlueSlot(1), CreateRedSlot(0), CreateRedSlot(1) }, result);
    }

    [Test]
    public void 선택에_따라_결과_반환()
    {
        TraitSelectionState sut = new(Team.Blue);

        TraitSelectResult result = sut.SelectTraitSlot(CreateSlot(Team.Red, 0));
        Assert.AreEqual(TraitSelectResult.Faild, result);

        result = sut.SelectTraitSlot(CreateSlot(Team.Blue, 0));
        Assert.AreEqual(TraitSelectResult.Select, result);

        result = sut.SelectTraitSlot(CreateSlot(Team.Red, 0));
        Assert.AreEqual(TraitSelectResult.Use, result);
    }

    SlotData CreateSlot(Team team, int index) => new SlotData(team, index);
}