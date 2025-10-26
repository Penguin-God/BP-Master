using NUnit.Framework;
using static TestHelper;

public class TraitFactoryTests
{
    [TestCase(TraitType.None, typeof(NullTrait))]
    [TestCase(TraitType.Charge, typeof(Charge))]
    [TestCase(TraitType.Guard, typeof(Guard))]
    public void Type에_맞는_Action_객체_생성(TraitType type, System.Type expectedType)
    {
        var sut = new TraitFactory(default, CreateOneSlotStatus());

        var result = sut.Create(Team.Blue, type);

        Assert.IsInstanceOf(expectedType, result);
    }

    [Test]
    public void Charge는_지정한_팀의_Charge_보유자에게만_같은_팀_보유_수_곱만큼_공격_증가()
    {
        var slots = CreateOneSlotStatus(traitType:TraitType.Charge);

        var config = new TraitConfig(5, 0f);
        var sut = new TraitFactory(config, slots);

        // Blue 팀에만 적용
        var trait = sut.Create(Team.Blue, TraitType.Charge);
        trait.Do();

        Assert.AreEqual(5, slots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(0, slots.GetSlot(RedZeroSlot).Stat.Attack);
    }
}
