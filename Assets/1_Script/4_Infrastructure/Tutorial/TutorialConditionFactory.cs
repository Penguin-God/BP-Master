using System;
using System.Linq;

public enum TutorialQuizType
{
    AttackAtLeast,
    TargetOpponent,
    TargetSelf
}

public record TutorialQuizConditionData(TutorialQuizType QuizType, int Threshold = 0);

public static class TutorialQuizConditionFactory
{
    public static Func<Champion, bool> Create(TutorialQuizConditionData data) => data.QuizType switch
    {
        TutorialQuizType.AttackAtLeast => champion => champion.Status.Stat.Attack >= data.Threshold,
        TutorialQuizType.TargetOpponent => champion => champion.Skill.Sides.Contains(Side.Opponent),
        TutorialQuizType.TargetSelf => champion => champion.Skill.Sides.Contains(Side.Self),
        _ => _ => false
    };
}
