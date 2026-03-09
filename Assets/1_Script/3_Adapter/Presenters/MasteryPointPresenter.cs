using System;

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

public interface IMasteryPointView
{
    void UpdatePoints(int points);
    void UpdateChampionDetail(ChampionTextModel champModel, MasteryLevelModel masteryModel);
    void ShowAlert(string message);
}

public class MasteryPointPresenter
{
    readonly MasteryInventory _inventory;
    readonly ChampionTextBuilder _championTextBuilder;
    readonly IMasteryPointView _view;

    int _currentSelectedId = -1;

    public MasteryPointPresenter(MasteryInventory inventory, ChampionTextBuilder championTextBuilder, IMasteryPointView view)
    {
        _inventory = inventory;
        _championTextBuilder = championTextBuilder;
        _view = view;
    }

    public void Initialize()
    {
        _view.UpdatePoints(_inventory.AvailablePoints);
    }

    public void SelectChampion(int id)
    {
        _currentSelectedId = id;
        RefreshDetail();
    }

    public void RequestUpgrade(StatType statType)
    {
        if (_currentSelectedId == -1) return;

        try
        {
            _inventory.Upgrade(_currentSelectedId, statType);

            _view.UpdatePoints(_inventory.AvailablePoints);
            RefreshDetail();
        }
        catch (InvalidOperationException ex)
        {
            _view.ShowAlert(ex.Message);
        }
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
}