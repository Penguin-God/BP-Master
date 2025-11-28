using NUnit.Framework;
using System.Collections.Generic;
using static TestHelper;

public class SKillDicisionTests
{
    [Test]
    public void 조건_불만족_시_스탯업()
    {
        var slots = CreateOneSlotStatus();
        var skill1 = CreateValueSkill(SkillType.AttackChanger, 100);
        var skill2 = CreateValueSkill(SkillType.DefenseChanger, 100, CreateThresholdCondition(StatConditionType.AttackBelow, 50), SelfAllRule);
        var skill3 = CreateValueSkill(SkillType.PercentAttackChanger, 100);

        var sut = new AI_SKillDicision();

        Assert.AreEqual(skill1, sut.SelectSkill(CreateSkills(skill1, skill2, skill3)));
        Assert.AreEqual(skill2, sut.SelectSkill(CreateSkills(skill2, skill3)));
    }

    IEnumerable<Skill> CreateSkills(params Skill[] skills) => skills;
}
