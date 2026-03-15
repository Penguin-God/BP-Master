using System.Linq;
using System.Collections.Generic;

public class MasteryTextBuilder
{
    readonly Dictionary<int, string> NameCatalog;
    public MasteryTextBuilder(Dictionary<int, string> nameCatalog) => NameCatalog = nameCatalog;

    public string BuildMasteriesText(IEnumerable<ChampionMastery> masteries)
        => string.Join("\n", masteries.Select(x =>$"{NameCatalog[x.ChampionId]} : {BuildMasteryStatText(x.MasteryStat)}"));

    string BuildMasteryStatText(ChampionStatData stat)
            => string.Join(", ", new[] { 
                FormatStat("공", stat.Attack), 
                FormatStat("방", stat.Defense), 
                FormatStat("속도", stat.Speed) 
            }.Where(x => x != null));

    string FormatStat(string prefix, int value) => value > 0 ? $"{prefix} {value}" : null;
}
