using System.Collections.Generic;
using System.Linq;

public class Skill
{
    public readonly IEnumerable<SkillData> SkillDatas;
    public Skill(IEnumerable<SkillData> skillDatas) => this.SkillDatas = skillDatas;

    public bool IsFreeCondition => SkillDatas.All(x => x.ConditionData.ConditionType == ConditionType.None);

    public IEnumerable<TraitTargetRule> Rules => SkillDatas.Select(x => x.TargetRule);
    public IEnumerable<Side> Sides => Rules.Select(x => x.TargetSide);
}
