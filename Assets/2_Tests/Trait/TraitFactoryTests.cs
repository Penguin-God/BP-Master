using NUnit.Framework;

public class TraitFactoryTests
{
    [Test]
    public void 팩토리로_공격력_증가_구성시_조건_만족이면_오버로드_ExecuteTrait이_적용된다()
    {
        var champion = TestHelper.CreateStatChamp(10, 5, 3);

        // 조건: AttackAtLeast 10 이상일 때만 동작, 액션: Attack +5
        var data = new TraitData(TraitType.AttackChanger, 5, TraitConditionType.AttackAtLeast, 10);
        var executor = TraitExecutorFactory.CreateExecutor(data);

        executor.ExecuteTrait(champion, data);

        Assert.AreEqual(15, champion.StatData.Attack);
        Assert.AreEqual(5, champion.StatData.Defense);
        Assert.AreEqual(3, champion.StatData.Speed);
    }

    [Test]
    public void 팩토리로_방어력_증가_구성시_조건_불만족이면_오버로드_ExecuteTrait이_변경하지_않는다()
    {
        var champion = TestHelper.CreateStatChamp(8, 4, 2);

        // 조건: DefenseAtLeast 10 이상이어야 하지만 현재 4 → 실행되지 않아야 함
        var data = new TraitData(TraitType.DefenseChanger, 10, TraitConditionType.DefenseAtLeast, 10);
        var executor = TraitExecutorFactory.CreateExecutor(data);

        executor.ExecuteTrait(champion, data);

        Assert.AreEqual(8, champion.StatData.Attack);
        Assert.AreEqual(4, champion.StatData.Defense); // unchanged
        Assert.AreEqual(2, champion.StatData.Speed);
    }
}
