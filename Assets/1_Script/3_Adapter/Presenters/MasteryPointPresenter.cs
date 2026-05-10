public readonly struct MasteryLevelModel
{
    public readonly string AttackText;
    public readonly string DefenseText;
    public readonly string SpeedText;

    public MasteryLevelModel(string attackText, string defenseText, string speedText)
    {
        AttackText = attackText;
        DefenseText = defenseText;
        SpeedText = speedText;
    }
}

public class MasteryPointPresenter
{
    readonly MasteryProfile _inventory;
    readonly ChampionTextBuilder _championTextBuilder;
    readonly IMasteryPointView _view;
    readonly IMasterySaver _saver;

    int _currentSelectedId = -1;

    public MasteryPointPresenter(MasteryProfile inventory, ChampionTextBuilder championTextBuilder, IMasteryPointView view, IMasterySaver saver)
    {
        _inventory = inventory;
        _championTextBuilder = championTextBuilder;
        _view = view;
        _saver = saver;
    }

    public void Initialize() => _view.UpdatePoints(_inventory.AvailablePoints);

    public void SelectChampion(int id)
    {
        _currentSelectedId = id;
        RefreshDetail();
    }

    public void RequestUpgrade(StatType statType)
    {
        if (_currentSelectedId == -1) return;

        _inventory.Upgrade(_currentSelectedId, statType);
        _view.UpdatePoints(_inventory.AvailablePoints);
        RefreshDetail();
        _saver.Save(_inventory);
    }

    void RefreshDetail()
    {
        var champModel = _championTextBuilder.Build(_currentSelectedId);
        var board = _inventory.GetBoard(_currentSelectedId);

        var masteryModel = new MasteryLevelModel(
            $"공 : {board.AttackLevel}",
            $"방 : {board.DefenseLevel}",
            $"속 : {board.SpeedLevel}"
        );

        _view.UpdateChampionDetail(champModel, masteryModel);
    }

    public void ResetMastery()
    {
        _inventory.ResetAll();
        _view.UpdatePoints(_inventory.AvailablePoints);

        if (_currentSelectedId != -1)
            RefreshDetail();

        _saver.Save(_inventory);
    }
}