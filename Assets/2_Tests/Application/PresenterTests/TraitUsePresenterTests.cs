using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class TraitUsePresenterTests
{
    TraitUsePresenter presenter;
    SlotStorage<ChampionStatus> statuses;

    [SetUp]
    public void SetUp()
    {
        SlotStorage<IEnumerable<TraitData>> picks = new();
        picks.AddSlot(Team.Blue, new TraitData[] { CreateTraitData(type: TraitType.AttackChanger, amount: 10), CreateTraitData(type: TraitType.DefenseChanger, amount: 10) });
        picks.AddSlot(Team.Blue, CreateTraitDatas(type: TraitType.AttackChanger, side: Side.Opponent, range: TargetRange.All, amount: 10));
        picks.AddSlot(Team.Blue, CreateTraitDatas(type: TraitType.AttackChanger, side: Side.Opponent, range: TargetRange.All, amount: 10));

        picks.AddSlot(Team.Red, CreateTraitDatas(type: TraitType.AttackChanger, side: Side.Self, range: TargetRange.Single, amount: 10));
        picks.AddSlot(Team.Red, CreateTraitDatas(type: TraitType.AttackChanger, side: Side.Self, range: TargetRange.Single, amount: 10));
        picks.AddSlot(Team.Red, CreateTraitDatas(type: TraitType.AttackChanger, side: Side.Self, range: TargetRange.Single, amount: 10));

        statuses = new();
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());
        statuses.AddSlot(Team.Blue, CreateStatus());

        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());
        statuses.AddSlot(Team.Red, CreateStatus());

        presenter = new TraitUsePresenter(new TraitUseFacade(statuses), picks, Team.Blue);
    }

    [Test]
    public void 특성_사용_적용()
    {
        presenter.ClickChampion(CreateSlot(Team.Blue, 0)); // 선택
        presenter.ClickChampion(CreateSlot(Team.Red, 0));  // 사용

        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Defense);
    }

    [Test]
    public void 클릭_상태에_따라_선택_가능한_슬롯들_반환()
    {
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 1, 2), presenter.GetClickableSlots());

        presenter.ClickChampion(CreateSlot(Team.Blue, 1));
        CollectionAssert.AreEquivalent(CreateRedSlots(0, 1, 2), presenter.GetClickableSlots());

        // blue 1번째 사용했으니 그건 제외
        presenter.ClickChampion(CreateSlot(Team.Red, 0));
        CollectionAssert.AreEquivalent(CreateBlueSlots(0, 2), presenter.GetClickableSlots());
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