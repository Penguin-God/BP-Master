public static class BanPickEnumCaster
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

    public static SelectType PhaseToSelect(GamePhase phase)
    {
        if (phase == GamePhase.Pick) return SelectType.Pick;
        else if (phase == GamePhase.Ban) return SelectType.Ban;
        else throw new System.Exception($"밴과 픽 중 하나여야 함 : {phase}");
    }
}
