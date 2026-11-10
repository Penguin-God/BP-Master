using UnityEngine;

public readonly struct StatChangeData
{
    public readonly ChampionStatData Before;
    public readonly ChampionStatData After;

    public StatChangeData(ChampionStatData before, ChampionStatData after)
    {
        Before = before;
        After = after;
    }
}


public class StatDeltaViewModel
{
    public bool IsChange { get; }
    public Color DeltaTextColor { get; }
    public string DeltaText { get; }
    
    public StatDeltaViewModel(bool isChange, Color deltaTextColor, string deltaText)
    {
        IsChange = isChange;
        DeltaTextColor = deltaTextColor;
        DeltaText = deltaText;
    }
}

public class StatChangeViewModel
{
    public StatDeltaViewModel Attack { get; }
    public StatDeltaViewModel Defense { get; }

    public StatChangeViewModel(StatDeltaViewModel attack, StatDeltaViewModel defense)
    {
        Attack = attack;
        Defense = defense;
    }
}

public class StatChangePresenter
{
    readonly Color positiveColor;
    readonly Color negativeColor;

    public StatChangePresenter(Color positiveColor, Color negativeColor)
    {
        this.positiveColor = positiveColor;
        this.negativeColor = negativeColor;
    }

    public StatChangeViewModel CreateViewModel(StatChangeData data)
    {
        var attackView = BuildDeltaView(data.Before.Attack, data.After.Attack);
        var defenseView = BuildDeltaView(data.Before.Defense, data.After.Defense);

        return new StatChangeViewModel(attackView, defenseView);
    }

    StatDeltaViewModel BuildDeltaView(int before, int after)
    {
        int delta = after - before;
        if (delta > 0) return new StatDeltaViewModel(isChange: true, positiveColor, $"+{delta}");
        else if (delta < 0) return new StatDeltaViewModel(isChange: true, negativeColor, $"-{Mathf.Abs(delta)}");
        else return new StatDeltaViewModel(isChange: false, Color.white, string.Empty);
    }
}
