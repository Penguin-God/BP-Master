public record MasteryPointModel(string PointText, ChampionStatModel ChampionStat, ChampionStatModel ChampionMastery);

public class MasteryPointPresenter
{
    readonly MasteryInventory _inventory;

    public MasteryPointPresenter(MasteryInventory inventory)
    {
        _inventory = inventory;
    }

    public ChampionStatModel GetMasteryPointModel(int championId)
    {
        var board = _inventory.GetBoard(championId);

        return new ChampionStatModel(
            $"공격Lv : {board.AttackLevel}",
            $"방어Lv : {board.DefenseLevel}",
            $"속도Lv : {board.SpeedLevel}"
        );
    }
}
