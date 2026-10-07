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
    readonly GameObject _uiTutorial;
    string[] _dialogues;
    public TutorialTrigger(string[] dialogues, GameObject uiTutorial)
    {
        _uiTutorial = uiTutorial;
        _dialogues = dialogues;
    }

    public void TriggerIfFirstTime(TutorialType type)
    {
        if (PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1) return;

        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
        GameObject.Instantiate(_uiTutorial).GetComponent<UI_Tutorial>().StartTutorial(_dialogues);
    }
}