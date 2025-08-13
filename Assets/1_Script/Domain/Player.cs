using System.Collections.Generic;
using System.Linq;

public class Player
{
    public readonly int Id;
    public readonly string PlayerName;
    HashSet<ChampionMastery> championMasteries;
    public IReadOnlyList<ChampionMastery> AllMasterys => championMasteries.ToArray();
    public Player(PlayerSO playerData)
    {
        Id = playerData.Id;
        PlayerName = playerData.PlayerName;
        championMasteries = playerData.StartMasteries.ToHashSet();
    }

    public ChampionMastery GetMastery(int chamId) => championMasteries.FirstOrDefault(x => x.Champion.Id == chamId);

    public void AddMastery(ChampionSO championSO)
    {
        if (GetMastery(championSO.Id) != null)
            GetMastery(championSO.Id).level++;
        else championMasteries.Add(new ChampionMastery(championSO, 1));
    }
}
