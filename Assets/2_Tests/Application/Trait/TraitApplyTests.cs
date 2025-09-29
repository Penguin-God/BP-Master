using NUnit.Framework;
using static TestHelper;

public class TraitApplyTests
{
    [Test]
    public void 조건은_실시간_반영()
    {
        SlotStorage<ChampionStatus> statuses = new();
        // Blue 1, Red 1 상태 초기화 (공격력 0)
        statuses.AddSlot(Team.Blue, CreateStatus(0));
        statuses.AddSlot(Team.Red, CreateStatus(0));

        var traitData = CreateAttTraitData(15, TraitConditionType.AttackBelow, 10, range: TargetRange.All);
        TraitApplier sut = new TraitApplier(statuses);

        sut.Execute(traitData, CreateRedSlot(0));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);

        // 조건이 거짓이므로 아무일도 안생김
        sut.Execute(traitData, CreateRedSlot(0));
        Assert.AreEqual(15, statuses.GetSlot(CreateRedSlot(0)).Stat.Attack);
    }
}
