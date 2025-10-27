using System;
using System.Linq;
using System.Collections.Generic;

public class MasteryTextBuilder
{
    readonly Dictionary<int, string> NameCatalog;
    public MasteryTextBuilder(Dictionary<int, string> nameCatalog) => NameCatalog = nameCatalog;

    public string BuildMasteriesText(IEnumerable<ChampionMastery> masteries)
        => string.Join(", ", masteries.Select(x =>$"{NameCatalog[x.ChampionId]} : {x.Level}"));
}
