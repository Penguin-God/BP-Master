using System.Collections.Generic;
using System.Linq;

public class Skill
{
    public readonly IEnumerable<SkillData> SkillDatas;
    public Skill(IEnumerable<SkillData> skillDatas) => this.SkillDatas = skillDatas;
    public IEnumerable<SkillTargetRule> Rules => SkillDatas.Select(x => x.TargetRule);
    public IEnumerable<Side> Sides => Rules.Select(x => x.TargetSide);
    public bool IsEmpty => SkillDatas.Count() == 0;
}
