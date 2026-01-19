using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class SkillRunnerTest
{
    [Test]
    [TestCase(10, 20, 130)] // 100 + 10 + 20 = 130
    [TestCase(5, 5, 110)]   // 100 + 5 + 5 = 110
    public void 여러_개의_스킬_데이터가_모두_적용되어야_함(int firstAmount, int secondAmount, int expectedAttack)
    {
        var caster = CreateStatus(100, 100, 100);
        var target = CreateStatus(100, 100, 100);

        var skill = CreateMultiAttackSkill(firstAmount, secondAmount);
        var sut = CreateSkillRunner();

        sut.Run(skill, caster, new[] { target });

        Assert.AreEqual(expectedAttack, target.Stat.Attack);
    }

    [Test]
    public void 빈_스킬은_아무런_동작도_하지_않아야_함()
    {
        var target = CreateStatus(100, 100, 100);
        var sut = CreateSkillRunner();

        sut.Run(CreateSkill(), null, new[] { target });

        Assert.AreEqual(100, target.Stat.Attack);
    }

    Skill CreateMultiAttackSkill(int amount1, int amount2)
    {
        var data1 = CreateValueSkillData(SkillType.AttackChanger, amount1, rule: AllRule);
        var data2 = CreateValueSkillData(SkillType.AttackChanger, amount2, rule: AllRule);

        return new Skill(new List<SkillData> { data1, data2 });
    }
}