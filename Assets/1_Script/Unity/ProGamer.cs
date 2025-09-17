using System.Collections.Generic;
using System.Linq;

public class ProGamer
{
    public readonly int Id;
    public readonly string PlayerName;
    HashSet<ChampionMasteryData> championMasteries;
    public IReadOnlyList<ChampionMasteryData> AllMasterys => championMasteries.ToArray();
    public ProGamer(PlayerSO playerData)
    {
        Id = playerData.Id;
        PlayerName = playerData.PlayerName;
        championMasteries = playerData.StartMasteries.ToHashSet();
    }

    public ChampionMasteryData GetMastery(int chamId) => championMasteries.FirstOrDefault(x => x.Champion.Id == chamId);

    public void AddMastery(ChampionSO championSO)
    {
        if (GetMastery(championSO.Id) != null)
            GetMastery(championSO.Id).level++;
        else championMasteries.Add(new ChampionMasteryData(championSO, 1));
    }
}
