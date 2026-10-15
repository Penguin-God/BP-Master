using UnityEngine;

public record CardUpgradeViewModel(int UpgradePoint, ChampionTextModel ChampionModel);

public class MasteryPointPresenter
{
    readonly MasteryProfile _inventory;
    readonly ChampionTextBuilder _championTextBuilder;
    readonly IMasterySaver _saver;
    readonly MasteryMultiplier _masteryMultiplier;

    public int CurrentPoint => _inventory.AvailablePoints;
    int _currentSelectedId = -1;

    public bool HasSelectedChampion => _currentSelectedId != -1;

    public MasteryPointPresenter(MasteryProfile inventory, ChampionTextBuilder championTextBuilder, IMasterySaver saver, MasteryMultiplier masteryMultiplier)
    {
        _inventory = inventory;
        _championTextBuilder = championTextBuilder;
        _saver = saver;
        _masteryMultiplier = masteryMultiplier;
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

    public int DowngradeCardStat(StatType statType)
    {
        if (HasSelectedChampion)
        {
            _inventory.Downgrade(_currentSelectedId, statType);
            _saver.Save(_inventory);
        }
        return _inventory.AvailablePoints;
    }

    public CardUpgradeViewModel CreateModel()
    {
        var board = _inventory.GetBoard(_currentSelectedId);
        var bonusStat = MasteryCalculator.CreateMasteryStat(_masteryMultiplier, board);
        var champModel = _championTextBuilder.Build(_currentSelectedId, bonusStat);
        return new CardUpgradeViewModel(_inventory.AvailablePoints, champModel);
    }

    public void ResetMastery()
    {
        _inventory.ResetAll();
        _saver.Save(_inventory);
    }

    public Color GetButtonColor(int id)
    {
        if (_inventory.GetBoard(id).IsUpgrade) return Color.green;
        else return Color.white;
    }
}