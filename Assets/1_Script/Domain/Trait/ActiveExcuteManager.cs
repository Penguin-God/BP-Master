using System;

public sealed class ActiveExcuteManager
{
    readonly ActiveExcuter _blue;
    readonly ActiveExcuter _red;

    public ActiveExcuteManager(ActiveExcuter blue, ActiveExcuter red)
    {
        _blue = blue;
        _red = red;
    }

    public bool IsDone => _blue.IsDone && _red.IsDone;
    public bool IsTeamDone(Team team) => team == Team.Blue ? _blue.IsTeamDone() : _red.IsTeamDone();
    public void DoActive(int championIndex, Team actingTeam, int[] targets = null)
    {
        switch (actingTeam)
        {
            case Team.Blue: _blue.DoActive(championIndex, targets); break;
            case Team.Red: _red.DoActive(championIndex, targets); break;
        }
    }
}
