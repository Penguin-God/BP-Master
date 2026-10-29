using System;
using System.Linq;

public enum TutorialQuizType
{
    SpeedAtLeast,
    TargetOpponent,
    TargetSelf
}

public record TutorialQuizConditionData(TutorialQuizType QuizType, int MinSpeed);

public static class TutorialQuizConditionFactory
{
    public static Func<Champion, bool> Create(TutorialQuizConditionData data) => data.QuizType switch
    {
        TutorialQuizType.SpeedAtLeast => champion => champion.Status.Stat.Speed >= data.MinSpeed,
        TutorialQuizType.TargetOpponent => champion => champion.Skill.Sides.Contains(Side.Opponent),
        TutorialQuizType.TargetSelf => champion => champion.Skill.Sides.Contains(Side.Self),
        _ => _ => false
    };
}