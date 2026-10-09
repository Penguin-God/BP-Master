using System;
using UnityEngine;

public static class TutorialEventBinder
{
    public static void BindBattleTutorial(Action<TutorialType> startTutorialOneTime, PhaseEventDispatcher eventDispatcher, BanPickStorage banPickStorage)
    {
        eventDispatcher.OnPhasePick += StartPickTutorial;
        eventDispatcher.OnPhaseBan += StartBanTutorial;

        void StartBanTutorial(Team team)
        {
            if (Team.Blue != team) return;
            startTutorialOneTime(TutorialType.MatchStart);
        }


        void StartPickTutorial(Team team)
        {
            if (Team.Blue != team) return;

            int pickCount = banPickStorage.PickIds.GetTeamCount(Team.Blue);
            switch (pickCount)
            {
                case 0: startTutorialOneTime(TutorialType.Pick); break;
                case 1: startTutorialOneTime(TutorialType.MasteryUIEnter); break;
                case 2: startTutorialOneTime(TutorialType.Swap); break;
            }
        }
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