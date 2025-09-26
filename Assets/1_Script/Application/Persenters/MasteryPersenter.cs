using System;
using System.Linq;
using System.Collections.Generic;

public class MasteryPersenter
{
    private readonly ChampionCatalog catalog;
    public MasteryPersenter(ChampionCatalog catalog) => this.catalog = catalog;

    public string BuildMasteriesText(IEnumerable<ChampionMastery> masteries)
        => string.Join(Environment.NewLine, masteries.Select(x =>$"{catalog.GetChampion(x.ChampionId).Name} : {x.Level}"));
    
}
