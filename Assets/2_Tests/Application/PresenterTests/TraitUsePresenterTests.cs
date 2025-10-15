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
    public void 특성_사용_데이터_반환()
    {
        TraitUseSlotData result;
        bool useable = presenter.ClickChampion(CreateSlot(Team.Blue, 0), out result);
        Assert.IsFalse(useable);
        useable = presenter.ClickChampion(CreateSlot(Team.Red, 0), out result);

        Assert.IsTrue(useable);
        Assert.AreEqual(CreateSlot(Team.Blue, 0), result.UseSlot);
        Assert.AreEqual(CreateSlot(Team.Red, 0), result.TargetSlot);
    }

    [Test]
    public void 선택에_따라_결과_반환()
    {
        TraitSelectionState sut = new(Team.Blue);
        SlotData useSlot = CreateSlot(Team.Blue, 0);

        SlotData? result = sut.ClickTraitSlot(CreateSlot(Team.Red, 0));
        Assert.AreEqual(null, result);

        result = sut.ClickTraitSlot(useSlot);
        Assert.AreEqual(null, result);

        result = sut.ClickTraitSlot(CreateSlot(Team.Red, 0));
        Assert.AreEqual(useSlot, result.Value);
    }

    SlotData CreateSlot(Team team, int index) => new SlotData(team, index);
}