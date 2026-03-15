using System.Collections.Generic;
using UnityEngine;

public class ChampionButtonStateModel
{
    public readonly string Name;
    public readonly Color32 ButtonColor;
    public readonly Color32 TextColor;
    public readonly bool IsEnabled;

    public ChampionButtonStateModel(string name, Color32 buttonColor, Color32 textColor, bool isEnabled)
    {
        Name = name;
        ButtonColor = buttonColor;
        TextColor = textColor;
        IsEnabled = isEnabled;
    }
}

public static class ChampionButtonPalette
{
    public static readonly Color32 BothMastered = new Color32(233, 233, 65, 255);  // 노란색
    public static readonly Color32 MyMastery = new Color32(111, 233, 65, 255); // 초록색
    public static readonly Color32 OpponentMastery = new Color32(233, 111, 111, 255); // 살짝 붉은색
    public static readonly Color32 DefaultButton = new Color32(255, 255, 255, 255);

    public static readonly Color32 ActiveText = new Color32(50, 50, 50, 255);
    public static readonly Color32 InactiveText = new Color32(60, 60, 60, 255);
}

public class ChampionButtonStatePresenter
{
    readonly HashSet<int> _myMasteryIds;
    readonly HashSet<int> _opponentMasteryIds;
    readonly HashSet<int> _selectableIds;
    readonly Dictionary<int, string> _nameCatalog;

    public ChampionButtonStatePresenter(IEnumerable<int> myMasteryIds,IEnumerable<int> opponentMasteryIds, IEnumerable<int> selectableIds, Dictionary<int, string> nameCatalog)
    {
        _myMasteryIds = new HashSet<int>(myMasteryIds);
        _opponentMasteryIds = new HashSet<int>(opponentMasteryIds);
        _selectableIds = new HashSet<int>(selectableIds);
        _nameCatalog = nameCatalog;
    }

    public ChampionButtonStateModel GetState(int championId)
    {
        string name = _nameCatalog.TryGetValue(championId, out var n) ? n : "Unknown";
        bool isEnabled = _selectableIds.Contains(championId);

        Color32 textColor = isEnabled ? ChampionButtonPalette.ActiveText : ChampionButtonPalette.InactiveText;
        Color32 buttonColor = DetermineButtonColor(championId);

        return new ChampionButtonStateModel(name, buttonColor, textColor, isEnabled);
    }

    Color32 DetermineButtonColor(int championId)
    {
        bool isMyMastery = _myMasteryIds.Contains(championId);
        bool isOpponentMastery = _opponentMasteryIds.Contains(championId);

        if (isMyMastery && isOpponentMastery) return ChampionButtonPalette.BothMastered;
        if (isMyMastery) return ChampionButtonPalette.MyMastery;
        if (isOpponentMastery) return ChampionButtonPalette.OpponentMastery;

        return ChampionButtonPalette.DefaultButton;
    }
}
