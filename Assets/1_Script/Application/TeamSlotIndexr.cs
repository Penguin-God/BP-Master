using System;

public class TeamSlotIndexr
{
    int blueIndex;
    int redIndex;

    public int GetNextIndex(Team team)
    {
        if (team == Team.Blue)
        {
            var result = blueIndex;
            blueIndex++;
            return result;
        }
        else if (team == Team.Red)
        {
            var result = redIndex;
            redIndex++;
            return result;
        }

        throw new ArgumentException($"Unknown team: {team}");
    }
}
