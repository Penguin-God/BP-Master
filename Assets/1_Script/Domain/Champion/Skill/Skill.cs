using System.Collections.Generic;
using System.Linq;

public class Skill
{
    public readonly IEnumerable<SkillData> SkillDatas;
    public Skill(IEnumerable<SkillData> skillDatas) => this.SkillDatas = skillDatas;

    public bool IsFreeCondition => SkillDatas.All(x => x.ConditionData.ConditionType == ConditionType.None);
}
