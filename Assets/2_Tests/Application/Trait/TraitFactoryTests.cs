using NUnit.Framework;
using static TestHelper;

public class TraitFactoryTests
{
    [TestCase(TraitType.None, typeof(NullTrait))]
    [TestCase(TraitType.Charge, typeof(Charge))]
    [TestCase(TraitType.Guard, typeof(Guard))]
    [TestCase(TraitType.Amplifier, typeof(Amplifier))]
    [TestCase(TraitType.Break, typeof(Break))]
    public void Type에_맞는_Action_객체_생성(TraitType type, System.Type expectedType)
    {
        var sut = new TraitFactory(default, CreateOneSlotStatus());

        var result = sut.Create(Team.Blue, type);

        Assert.IsInstanceOf(expectedType, result);
    }

    [Test]
    public void 팩토리에_넘긴_변수에_맞게_특성_적용()
    {
        var slots = CreateOneSlotStatus(traitType:TraitType.Charge);

        var config = CreateTraitConfig(chargeAttack: 5);
        var sut = new TraitFactory(config, slots);

        // Blue 팀에만 적용
        var trait = sut.Create(Team.Blue, TraitType.Charge);
        trait.Do();

        Assert.AreEqual(5, slots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(0, slots.GetSlot(RedZeroSlot).Stat.Attack);
    }

    [Test]
    public void 타겟이_상대팀이면_그에맞게_적용()
    {
        var slots = CreateOneSlotStatus();

        var config = CreateTraitConfig(breakRate: 0.2f);
        var sut = new TraitFactory(config, slots);

        var trait = sut.Create(Team.Blue, TraitType.Break);
        trait.Do();

        // Red 팀에만 적용
        Assert.AreEqual(1, slots.GetSlot(BlueZeroSlot).DownRate);
        Assert.AreEqual(1.2f, slots.GetSlot(RedZeroSlot).DownRate);
    }
}
