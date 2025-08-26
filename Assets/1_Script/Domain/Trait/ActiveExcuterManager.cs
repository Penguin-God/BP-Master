using System;

public sealed class ActiveExcuterManager
{
    readonly ActiveExcuter _blue;
    readonly ActiveExcuter _red;

    public ActiveExcuterManager(ActiveExcuter blue, ActiveExcuter red)
    {
        _blue = blue ?? throw new ArgumentNullException(nameof(blue));
        _red = red ?? throw new ArgumentNullException(nameof(red));
    }

    public bool IsDone => _blue.IsDone && _red.IsDone;

    public void DoActive(int targetIndex, Team actingTeam)
    {
        switch (actingTeam)
        {
            case Team.Blue: _blue.DoActive(targetIndex); break;
            case Team.Red: _red.DoActive(targetIndex); break;
        }
    }
}
