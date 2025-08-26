using System;

public static class TeamSideConverter
{
    public static Team GetTargetTeam(Team selfTeam, Side side)
    {
        if (selfTeam == Team.All || side == Side.All) return Team.All;

        switch (selfTeam)
        {
            case Team.Blue: return side == Side.Self ? Team.Blue : Team.Red;
            case Team.Red: return side == Side.Self ? Team.Red : Team.Blue;
        }
        return Team.All;
    }
}
