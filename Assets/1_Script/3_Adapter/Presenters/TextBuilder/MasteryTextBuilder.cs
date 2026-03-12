using System.Linq;
using System.Collections.Generic;

public class MasteryTextBuilder
{
    readonly Dictionary<int, string> NameCatalog;
    public MasteryTextBuilder(Dictionary<int, string> nameCatalog) => NameCatalog = nameCatalog;

    public string BuildMasteriesText(IEnumerable<ChampionMastery> masteries)
        => string.Join("\n", masteries.Select(x =>$"{NameCatalog[x.ChampionId]} : {BuildMasteryStatText(x.MasteryStat)}"));

    string BuildMasteryStatText(ChampionStatData stat) => $"공 {stat.Attack}, 방 {stat.Defense}, 속도 {stat.Speed}";
}
