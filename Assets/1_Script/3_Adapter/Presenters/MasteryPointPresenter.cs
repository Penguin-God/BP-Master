using UnityEngine;

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

public record CardUpgradeViewModel(int UpgradePoint, MasteryLevelModel MasteryModel, ChampionTextModel ChampionModel);

public class MasteryPointPresenter
{
    readonly MasteryProfile _inventory;
    readonly ChampionTextBuilder _championTextBuilder;
    readonly IMasterySaver _saver;
    readonly MasteryApplier _masteryApplier;

    public int CurrentPoint => _inventory.AvailablePoints;
    int _currentSelectedId = -1;

    public bool HasSelectedChampion => _currentSelectedId != -1;
    public MasteryPointPresenter(MasteryProfile inventory, ChampionTextBuilder championTextBuilder, IMasterySaver saver)
    {
        _inventory = inventory;
        _championTextBuilder = championTextBuilder;
        _saver = saver;
    }

    public MasteryPointPresenter(MasteryProfile inventory, ChampionTextBuilder championTextBuilder, IMasterySaver saver, MasteryApplier masteryApplier)
    {
        _inventory = inventory;
        _championTextBuilder = championTextBuilder;
        _saver = saver;
        _masteryApplier = masteryApplier;
    }

    public void SelectChampion(int id) => _currentSelectedId = id;

    public int UpgradeCardStat(StatType statType)
    {
        if (HasSelectedChampion)
        {
            _inventory.Upgrade(_currentSelectedId, statType);
            _saver.Save(_inventory);
        }
        return _inventory.AvailablePoints;
    }

    public CardUpgradeViewModel CreateModel()
    {
        var bonusStat = _masteryApplier.GetBonusStat(_currentSelectedId);
        var champModel = _championTextBuilder.Build(_currentSelectedId, bonusStat);
        var board = _inventory.GetBoard(_currentSelectedId);
        return new CardUpgradeViewModel(_inventory.AvailablePoints, MasteryLevelModelCreator.CreateModel(board), champModel);
    }

    public void ResetMastery()
    {
        _inventory.ResetAll();
        _saver.Save(_inventory);
    }

    public Color GetButtonColor(int id)
    {
        if (_inventory.TryGetBoard(id, out _)) return Color.green;
        else return Color.white;
    }
}

public static class MasteryLevelModelCreator
{
    public static MasteryLevelModel CreateModel(MasteryBoard board) => new MasteryLevelModel(
            $"공 : {board.AttackLevel}",
            $"방 : {board.DefenseLevel}",
            $"속 : {board.SpeedLevel}"
        );
}