using System.Linq;

public class ChampionStorageFactory
{
    readonly ChampionCatalog championCatalog;

    public ChampionStorageFactory(ChampionCatalog championCatalog)
    {
        this.championCatalog = championCatalog;
    }

    public SlotStorage<ChampionStatus> CreateStatusStorage(SlotStorage<int> idStorage)
    {
        var result = new SlotStorage<ChampionStatus>();

        // 팀별 순서 보존하여 채우기
        foreach (var team in new[] { Team.Blue, Team.Red })
        {
            var statuses = idStorage
                .GetTeam(team)
                .Select(id => new ChampionStatus(championCatalog.GetChampion(id).StatData));

            result.AddSlots(team, statuses);
        }

        return result;
    }

    public SlotStorage<Champion> CreateChampionStorage(SlotStorage<int> idStorage)
    {
        var result = new SlotStorage<Champion>();

        foreach (var team in new[] { Team.Blue, Team.Red })
        {
            var champions = idStorage
                .GetTeam(team)
                .Select(id => championCatalog.GetChampion(id));

            result.AddSlots(team, champions);
        }

        return result;
    }
}
