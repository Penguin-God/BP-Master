using System;
using System.Linq;
using System.Collections.Generic;

public static class EnumCaster
{
    public static Team GetTargetTeam(Team selfTeam, Side side) => (selfTeam, side) switch
    {
        (Team.Blue, Side.Self) => Team.Blue,
        (Team.Blue, Side.Opponent) => Team.Red,
        (Team.Red, Side.Self) => Team.Red,
        (Team.Red, Side.Opponent) => Team.Blue,
        (_, Side.All) => Team.All,

        _ => throw new ArgumentOutOfRangeException(nameof(side), $"없는 조합: {selfTeam}, {side}")
    };

    public static SelectType PhaseToSelect(GamePhase phase)
    {
        if (phase == GamePhase.Pick) return SelectType.Pick;
        else if (phase == GamePhase.Ban) return SelectType.Ban;
        else throw new Exception($"밴과 픽 중 하나여야 함 : {phase}");
    }

    public static Side MergeSide(IEnumerable<Side> sides)
    {
        if (sides.All(s => s == Side.Self)) return Side.Self;
        else if (sides.All(s => s == Side.Opponent)) return Side.Opponent;
        else return Side.All;
    }

    public static Team GetOppoentTeam(Team team)
    {
        if (team == Team.Blue) return Team.Red;
        else if (team == Team.Red) return Team.Blue;
        else throw new Exception("All은 안됨");
    }
}
