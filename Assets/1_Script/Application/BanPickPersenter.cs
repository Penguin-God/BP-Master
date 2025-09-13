using System;

public class BanPickPersenter
{
    int blueIndex;
    int redIndex;

    public ChampionSlot GetNextSlot(Team team)
    {
        if (team == Team.Blue)
        {
            var slot = new ChampionSlot(Team.Blue, blueIndex);
            blueIndex++;
            return slot;
        }
        else if (team == Team.Red)
        {
            var slot = new ChampionSlot(Team.Red, redIndex);
            redIndex++;
            return slot;
        }

        throw new ArgumentException($"Unknown team: {team}");
    }
}
