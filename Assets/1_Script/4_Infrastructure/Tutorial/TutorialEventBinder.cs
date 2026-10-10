using System;
using UnityEngine;

public enum TutorialTriggerType
{
    MatchStart,
    MasteryUIEnter,
    FirstPick,
    SecondPick,
    ThreedPick,
    Swap,
}

public static class TutorialActor
{
    public static void StartIfFirstTime(TutorialTriggerType type, Action tutorial)
    {
        if (PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1) return;

        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
        tutorial?.Invoke();
    }
}