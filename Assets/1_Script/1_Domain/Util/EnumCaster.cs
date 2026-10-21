using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumCaster
{
    public static Team GetTargetTeam(Team selfTeam, Side side) => (selfTeam, side) switch
    {
        (Team.Blue, Side.Self) => Team.Blue,
        (Team.Blue, Side.Opponent) => Team.Red,
        (Team.Red, Side.Self) => Team.Red,
        (Team.Red, Side.Opponent) => Team.Blue,
        (_, Side.All) => Team.All,
        (_, Side.OneSelf) => selfTeam,

        _ => throw new ArgumentOutOfRangeException(nameof(side), $"없는 조합: {selfTeam}, {side}")
    };

    public static Side MergeSide(IEnumerable<Side> sides)
    {
        if(sides.Count() == 1) return sides.First();
        else if (sides.Contains(Side.Self) && sides.Contains(Side.Opponent)) return Side.All;
        else if(sides.Contains(Side.All)) return Side.All;
        else if (sides.Distinct().Count() > 1) throw new Exception($"허용되지 않는 Side 조합 : {string.Join(", ", sides)}");
        else return sides.First();
    }

    public static CardSelectRule MergeRule(IEnumerable<CardSelectRule> rules)
    {
        if (rules.Count() == 1) return rules.First();
        if (rules.Select(x => x.TargetRange).Distinct().Count() > 1) throw new Exception($"range가 통일되지 않음 : {string.Join(", ", rules.Select(x => x.TargetRange))}");
        if (rules.Count() == 0) return default;
        return new CardSelectRule(MergeSide(rules.Select(x => x.TargetSide)), rules.First().TargetRange);
    }

    public static Team GetOppoentTeam(Team team)
    {
        if (team == Team.Blue) return Team.Red;
        else if (team == Team.Red) return Team.Blue;
        else throw new Exception("All은 안됨");
    }
}
