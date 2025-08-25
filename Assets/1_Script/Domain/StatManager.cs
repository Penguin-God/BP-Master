using System;
using System.Collections.Generic;
using System.Linq;

public enum Side { Self, Opponent, All }
public class StatManager
{
    ChampionStatData[] self;
    ChampionStatData[] opponent;
    public IReadOnlyList<ChampionStatData> Self => self;
    public IReadOnlyList<ChampionStatData> Opponent => opponent;

    public StatManager(IEnumerable<ChampionStatData> self, IEnumerable<ChampionStatData> opponent)
    {
        this.self = self.ToArray();
        this.opponent = opponent.ToArray();
    }


    public IReadOnlyList<ChampionStatData> GetData(Side side) => side == Side.Self ? self : opponent;

    public void ChangeSelectData(Side side, int index, Func<ChampionStatData, ChampionStatData> mutator)
    {
        var arr = side == Side.Self ? self : opponent;
        arr[index] = mutator(arr[index]);
    }

    public void ChangeAll(Side side, Func<ChampionStatData, ChampionStatData> mutator)
    {
        if (side == Side.Self) self = self.Select(mutator).ToArray();
        else if(side == Side.Opponent) opponent = opponent.Select(mutator).ToArray();
    }
}
