using NUnit.Framework;
using static TestHelper;

public class TraitExecuteTests
{
    [Test]
    public void 양팀_각각의_특성은_자기_팀에게만_적용된다()
    {
        var slots = CreateOneSlotStatus(traitType: TraitType.Charge);
        var config = CreateTraitConfig(chargeAttack: 5);
        var factory = new TraitFactory(config, slots);
        var sut = new TraitExecutor(factory);

        sut.ExecuteAllTriat(slots);

        Assert.AreEqual(5, slots.GetSlot(BlueZeroSlot).Stat.Attack);
        Assert.AreEqual(5, slots.GetSlot(RedZeroSlot).Stat.Attack);
    }
}
