using System.Linq;

public class TeamMasteryApplier
{
    readonly SlotStorage<ProGamer> gamers;
    readonly SlotStorage<int> ids;
    readonly SlotStatusChanger statusChanger;

    public TeamMasteryApplier(SlotStorage<ProGamer> gamers, SlotStorage<int> ids, SlotStatusChanger statusChanger)
    {
        this.gamers = gamers;
        this.ids = ids;
        this.statusChanger = statusChanger;
    }

    public void Apply(Team team)
    {
        var finder = new ActiveMasteryFinder(gamers, ids);

        // 사용자의 스타일(반복문 대신 LINQ 지향)에 맞춰 인덱스 포함 열거
        gamers.GetTeam(team)
              .Select((_, index) => new SlotData(team, index))
              .ToList()
              .ForEach(slot =>
              {
                  int level = finder.GetActiveLevel(slot);
                  statusChanger.ChangeStat(slot, stat => new MasteryApplier().ApplyMastery(stat, level));
              });
    }
}
