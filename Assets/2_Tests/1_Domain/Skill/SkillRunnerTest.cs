using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class SkillRunnerTest
{
    [Test]
    [TestCase(10, 20, 130)]
    [TestCase(5, 5, 110)]
    public void 여러_개의_스킬_데이터가_모두_적용되어야_함(int firstAmount, int secondAmount, int expectedAttack)
    {
        var caster = CreateStatus(100, 100, 100);
        var target = CreateStatus(100, 100, 100);

        var skill = CreateMultiAttackSkill(firstAmount, secondAmount);
        var sut = CreateSkillRunner();

        sut.Run2(skill, caster, new[] { target }, Team.Blue);

        Assert.AreEqual(expectedAttack, target.Stat.Attack);
    }

    [Test]
    public void 선택_스탯_참조로_본인에_사용()
    {
        var caster = CreateStatus(100, 100, 100);
        var target = CreateStatus(0, 200, 0);

        var skill = CreateSkill(new SkillData(SkillType.StatChanger, CalculatorFactory.CreatePercent(0.2f), SkillCondtionFactory.CreateNull(), default, SkillApplyType.OneSelf, SkillValueType.SelectRef, StatType.Defense));
        var sut = CreateSkillRunner();

        sut.Run2(skill, caster, new[] { target }, 0);

        Assert.AreEqual(140, caster.Stat.Defense);
    }

    [Test]
    public void 스탯_본인_참조로_선택에_적용()
    {
        var caster = CreateStatus(100, 100, 100);
        var select = CreateStatus(0, 200, 0);

        var skill = CreateSkill(new SkillData(SkillType.StatChanger, CalculatorFactory.CreatePercent(0.2f), SkillCondtionFactory.CreateNull(), default, SkillApplyType.Select, SkillValueType.SelfRef, StatType.Defense));
        var sut = CreateSkillRunner();

        sut.Run2(skill, caster, new[] { select }, 0);

        Assert.AreEqual(220, select.Stat.Defense);
    }

    [Test]
    public void 고정값_스킬은_대상의_스탯을_해당_값으로_고정()
    {
        var caster = CreateStatus(100, 100, 100);
        var target = CreateStatus(100, 100, 100);

        var skill = CreateSkill(new SkillData(SkillType.StatChanger, CalculatorFactory.CreateFix(5), SkillCondtionFactory.CreateNull(), default, SkillApplyType.Select, SkillValueType.Value, StatType.Speed));
        var sut = CreateSkillRunner();

        sut.Run2(skill, caster, new[] { target }, Team.Blue);

        Assert.AreEqual(5, target.Stat.Speed);
    }

    [Test]
    public void 고정값_스킬이_자신에게_적용될_때도_자신의_스탯을_해당_값으로_고정()
    {
        var caster = CreateStatus(100, 100, 100);

        var skill = CreateSkill(new SkillData(SkillType.StatChanger, CalculatorFactory.CreateFix(5), SkillCondtionFactory.CreateNull(), default, SkillApplyType.OneSelf, SkillValueType.Value, StatType.Speed));
        var sut = CreateSkillRunner();

        sut.Run2(skill, caster, new[] { caster }, Team.Blue);

        Assert.AreEqual(5, caster.Stat.Speed);
    }

    [Test]
    public void 빈_스킬은_아무런_동작도_하지_않아야_함()
    {
        var target = CreateStatus(100, 100, 100);
        var sut = CreateSkillRunner();

        sut.Run2(CreateSkill(), null, new[] { target }, Team.Blue);

        Assert.AreEqual(100, target.Stat.Attack);
    }

    Skill CreateMultiAttackSkill(int amount1, int amount2)
    {
        var data1 = CreateAttackChangeSkill(amount1, rule: AllRule);
        var data2 = CreateAttackChangeSkill(amount2, rule: AllRule);

        return new Skill(new List<SkillData> { data1, data2 });
    }
}
