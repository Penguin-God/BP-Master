using System;
using System.Collections.Generic;
using System.Linq;

public class MasteryDrawer
{
    readonly ChampionCatalog catalog;
    
    public MasteryDrawer(ChampionCatalog catalog)
    {
        this.catalog = catalog;
    }

    public ChampionMastery[] DrawRandoms(int[] levels)
    {
        var allIds = catalog.AllId.ToList();
        var results = new List<ChampionMastery>();

        foreach (var level in levels)
        {
            if (allIds.Count == 0) throw new InvalidOperationException("더 이상 선택할 수 있는 챔피언이 없습니다.");

            int index = GetRandomIndex(allIds.Count);
            int id = allIds[index];
            allIds.RemoveAt(index);

            results.Add(new ChampionMastery(id, level));
        }

        return results.ToArray();
    }

    readonly Random random = new Random();
    int GetRandomIndex(int listCount) => random.Next(listCount);
}
