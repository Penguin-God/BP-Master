using UnityEngine;
using System;

public static class TutorialEventBinder
{
    public static void BindBattleTutorial(Action<TutorialType> startTutorialOneTime, int totalWins)
    {
        if (totalWins == 0) startTutorialOneTime(TutorialType.MatchStart);
        else startTutorialOneTime(TutorialType.SecondSetEnter);


    }
}

public enum TutorialType
{
    MatchStart,
    SecondSetEnter,
    MasteryUIEnter
}

public class TutorialTrigger
{
    readonly System.Action<TutorialType> _showTutorialUI;

    public TutorialTrigger(System.Action<TutorialType> showTutorialUI) => _showTutorialUI = showTutorialUI;

    public void TriggerIfFirstTime(TutorialType type)
    {
        if (PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1) return;

        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
        _showTutorialUI(type);
    }
}