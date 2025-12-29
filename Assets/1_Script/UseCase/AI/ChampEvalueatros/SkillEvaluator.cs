using System;
using System.Linq;

public class SkillEvaluator
{
    SlotStorage<ChampionStatus> statuses;
    readonly int TeamSize;
    const double EmptySlotHalfWeight = 0.5;


    public SkillEvaluator(SlotStorage<ChampionStatus> statuses, int teamSize)
    {
        this.statuses = statuses;
        this.TeamSize = teamSize;
    }

    public int Evaluate(SkillData skill, Team team)
    {
        int sign = skill.TargetRule.TargetSide == Side.Opponent ? -1 : 1;
        if (skill.ConditionData.ConditionType == ConditionType.None)
        {
            if (skill.TargetRule.TargetSide == Side.All) return 0;
            return sign * skill.AmountData.ValueAmount * TeamSize;
        }
        else return GetConditionValueSum(skill, team);
    }

    int GetConditionValueSum(SkillData skill, Team team)
    {
        switch (skill.TargetRule.TargetSide)
        {
            case Side.Self: return GetConditionValue(skill, team);
            case Side.Opponent: return GetOppentConditionValue(skill, team);
            case Side.All: return GetConditionValue(skill, team) + GetOppentConditionValue(skill, team);
        }
        return 0;
    }

    int GetOppentConditionValue(SkillData skill, Team team) => GetConditionValue(skill, EnumCaster.GetOppoentTeam(team)) * -1;

    int GetConditionValue(SkillData skill, Team targetTeam)
    {
        int pickSlotValue = skill.AmountData.ValueAmount * statuses.GetTeam(targetTeam).Where(x => SkillCondtionFactory.CreateCondition(skill.ConditionData, default).Check(x)).Count();
        return pickSlotValue + CalculateEmptySlotValue(targetTeam, skill);
    }

    int CalculateEmptySlotValue(Team targetTeam, SkillData skill)
    {
        int currentPickedCount = statuses.GetTeamCount(targetTeam);
        int emptySlotCount = TeamSize - currentPickedCount;
        return (int)Math.Round(skill.AmountData.ValueAmount * EmptySlotHalfWeight * emptySlotCount, MidpointRounding.AwayFromZero); 
    }
}

public record TeamStatChangeInfo(int Att, int Def, int Speed);
public record GameStatChangeInfo(TeamStatChangeInfo Blue, TeamStatChangeInfo Red);
