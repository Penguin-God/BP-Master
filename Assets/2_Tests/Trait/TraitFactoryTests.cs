using NUnit.Framework;

public class TraitFactoryTests
{
    [Test]
    public void 팩토리로_공격력_증가_구성시_조건_만족이면_오버로드_ExecuteTrait이_적용된다()
    {
        var champion = TestHelper.CreateStatChamp(10, 5, 3);

        // 조건: AttackAtLeast 10 이상일 때만 동작, 액션: Attack +5
        var executor = TraitExecutorFactory.Create(
            actionType: TraitType.AttackChanger,
            amount: 5,
            conditionType: TraitConditionType.AttackAtLeast,
            threshold: 10
        );

        executor.ExecuteTrait(champion);

        Assert.AreEqual(15, champion.StatData.Attack);
        Assert.AreEqual(5, champion.StatData.Defense);
        Assert.AreEqual(3, champion.StatData.Speed);
    }

    [Test]
    public void 팩토리로_방어력_증가_구성시_조건_불만족이면_오버로드_ExecuteTrait이_변경하지_않는다()
    {
        var champion = TestHelper.CreateStatChamp(8, 4, 2);

        // 조건: DefenseAtLeast 10 이상이어야 하지만 현재 4 → 실행되지 않아야 함
        var executor = TraitExecutorFactory.Create(
            actionType: TraitType.DefenseChanger,
            amount: 10,
            conditionType: TraitConditionType.DefenseAtLeast,
            threshold: 10
        );

        executor.ExecuteTrait(champion);

        Assert.AreEqual(8, champion.StatData.Attack);
        Assert.AreEqual(4, champion.StatData.Defense); // unchanged
        Assert.AreEqual(2, champion.StatData.Speed);
    }
}
