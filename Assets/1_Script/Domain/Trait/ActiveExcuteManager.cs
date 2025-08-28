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
    public bool IsTeamDone(Team team) => team == Team.Blue ? _blue.IsDone : _red.IsDone;
    public void DoActive(int targetIndex, Team actingTeam)
    {
        switch (actingTeam)
        {
            case Team.Blue: _blue.DoActive(targetIndex); break;
            case Team.Red: _red.DoActive(targetIndex); break;
        }
    }
}
