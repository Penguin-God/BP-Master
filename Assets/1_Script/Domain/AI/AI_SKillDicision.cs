using System.Collections.Generic;
using System.Linq;

public class AI_SKillDicision
{
    public Skill SelectSkill(IEnumerable<Skill> skills)
    {
        var list = skills.ToList();

        var first = list.Where(s => s.IsFreeCondition && HasType(s, SkillType.AttackChanger, SkillType.DefenseChanger)).ToList();
        if (first.Count > 0)
            return RandomUtil.DrawRandom(first);

        var second = list.Where(s => !s.IsFreeCondition && HasType(s, SkillType.AttackChanger, SkillType.DefenseChanger)).ToList();
        if (second.Count > 0)
            return RandomUtil.DrawRandom(second);

        return RandomUtil.DrawRandom(list);
    }

    bool HasType(Skill skill, params SkillType[] types)
    {
        foreach (var data in skill.SkillDatas)
        {
            if (types.Contains(data.TraitType))
                return true;
        }
        return false;
    }
}
