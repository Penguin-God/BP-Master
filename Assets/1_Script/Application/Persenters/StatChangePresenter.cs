using System.Collections.Generic;
using System.Linq;
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
    public IEnumerable<int> DeltaValues { get; }

    public StatDeltaViewModel(bool isChange, Color deltaTextColor, string deltaText, IEnumerable<int> deltaValues)
    {
        IsChange = isChange;
        DeltaTextColor = deltaTextColor;
        DeltaText = deltaText;
        DeltaValues = deltaValues;
    }
}

public class StatChangeViewModel
{
    public StatDeltaViewModel Attack { get; }
    public StatDeltaViewModel Defense { get; }
    public StatDeltaViewModel Speed { get; }

    public StatChangeViewModel(StatDeltaViewModel attack, StatDeltaViewModel defense, StatDeltaViewModel speed)
    {
        Attack = attack;
        Defense = defense;
        Speed = speed;
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
        var speedView = BuildDeltaView(data.Before.Speed, data.After.Speed);

        return new StatChangeViewModel(attackView, defenseView, speedView);
    }

    StatDeltaViewModel BuildDeltaView(int before, int after)
    {
        int delta = after - before;
        if (delta > 0)
        {
            var deltaValues = Enumerable.Range(before + 1, delta);
            return new StatDeltaViewModel(true, positiveColor, $"+{delta}", deltaValues);
        }
        else if (delta < 0)
        {
            int count = Mathf.Abs(delta);
            var deltaValues = Enumerable.Range(after, count).Reverse(); // 내림차순
            return new StatDeltaViewModel(true, negativeColor, $"-{count}", deltaValues);
        }
        else return new StatDeltaViewModel(false, Color.white, string.Empty, null);
    }
}