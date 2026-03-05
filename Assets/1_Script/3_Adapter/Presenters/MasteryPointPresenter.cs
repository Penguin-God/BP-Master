public record MasteryLevelModel(string AttackText, string DefenseText, string SpeedText);

public class MasteryPointPresenter
{
    readonly MasteryInventory _inventory;

    public MasteryPointPresenter(MasteryInventory inventory)
    {
        _inventory = inventory;
    }

    public MasteryLevelModel GetMasteryPointModel(int championId)
    {
        var board = _inventory.GetBoard(championId);

        return new MasteryLevelModel(
            $"공격Lv : {board.AttackLevel}",
            $"방어Lv : {board.DefenseLevel}",
            $"속도Lv : {board.SpeedLevel}"
        );
    }
}
