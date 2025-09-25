using System;
using System.Linq;
using System.Collections.Generic;

public class MasteryPersenter
{
    private readonly ChampionCatalog catalog;
    public MasteryPersenter(ChampionCatalog catalog) => this.catalog = catalog;

    public string Present(Dictionary<int, int> mastery)
        => string.Join(Environment.NewLine, mastery.Select(pair =>$"{catalog.GetChampion(pair.Key).Name} : {pair.Value}"));
    
}
