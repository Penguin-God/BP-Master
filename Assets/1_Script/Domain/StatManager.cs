using System;
using System.Collections.Generic;
using System.Linq;

public enum Side { Self, Opponent, All }
public class StatManager
{
    ChampionStatData[] blue;
    ChampionStatData[] red;
    public IReadOnlyList<ChampionStatData> Blue => blue;
    public IReadOnlyList<ChampionStatData> Red => red;

    public StatManager(IEnumerable<ChampionStatData> blue, IEnumerable<ChampionStatData> red)
    {
        this.blue = blue.ToArray();
        this.red = red.ToArray();
    }

    public void ChangeSelectData(Team team, int index, Func<ChampionStatData, ChampionStatData> mutator)
    {
        var arr = team == Team.Blue ? blue : red;
        arr[index] = mutator(arr[index]);
    }

    public void ChangeAll(Team team, Func<ChampionStatData, ChampionStatData> mutator)
    {
        if (team == Team.Blue) blue = blue.Select(mutator).ToArray();
        else if(team == Team.Red) red = red.Select(mutator).ToArray();
    }
}
