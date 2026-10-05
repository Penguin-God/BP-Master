using UnityEngine;
using System;

public static class TutorialEventBinder
{
    public static void BindBattleTutorial(Action<TutorialType> startTutorialOneTime, PhaseEventDispatcher eventDispatcher)
    {
        eventDispatcher.OnPhasePick += (Team team) => startTutorialOneTime(TutorialType.Pick);
        eventDispatcher.OnPhaseBan += (Team team) => startTutorialOneTime(TutorialType.MatchStart);
    }
}

public enum TutorialType
{
    MatchStart,
    MasteryUIEnter,
    Pick,
    Swap,
}

public class TutorialTrigger
{
    readonly Action<TutorialType> _showTutorialUI;
    public TutorialTrigger(Action<TutorialType> showTutorialUI) => _showTutorialUI = showTutorialUI;

    public void TriggerIfFirstTime(TutorialType type)
    {
        if (PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1) return;

        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
        _showTutorialUI(type);
    }
}