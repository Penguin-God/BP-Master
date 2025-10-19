using NUnit.Framework;
using static TestHelper;

public class TraitFacadeTests
{
    [Test]
    public void 특성_컬랙션_적용()
    {
        SlotStorage<ChampionStatus> statuses = CreateOneSlotStatus();
        SlotStorage<TraitApplier> appliers = CreateOneSlotApplier(statuses);
        TraitData[] datas = new TraitData[] { CreateTraitData(TraitType.AttackChanger, 10, range: TargetRange.Single), CreateTraitData(TraitType.DefenseChanger, 10, side: Side.Self, range: TargetRange.Single) };
        var sut = new TraitUseFacade(appliers, statuses);

        sut.UseTrait(CreateBlueSlot(0), CreateRedSlot(0), datas);

        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(CreateRedSlot(0)).Stat.Defense);
        Assert.IsTrue(appliers.GetSlot(CreateBlueSlot(0)).IsUse);
    }

    [Test]
    public void 특성_적용()
    {
        SlotStorage<ChampionStatus> statuses = CreateOneSlotStatus();
        SlotStorage<TraitApplier> appliers = CreateOneSlotApplier(statuses);
        TraitData[] datas = CreateTraits(CreateConditionFreeTrait(TraitType.AttackChanger, 10, SelfAllRule), CreateConditionFreeTrait(TraitType.DefenseChanger, 10, SelfAllRule));
        var sut = new TraitUseFacade(appliers, statuses);
        SlotData callSlot = RedOneSlot;
        sut.OnUseTrait += slot => callSlot = slot;

        sut.UseTrait(BlueZeroSlot, new SlotData[] { BlueZeroSlot }, datas);

        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(10, statuses.GetSlot(BlueZeroSlot).Stat.Defense);
        Assert.IsTrue(appliers.GetSlot(CreateBlueSlot(0)).IsUse);
        Assert.AreEqual(BlueZeroSlot, callSlot);
    }
}
