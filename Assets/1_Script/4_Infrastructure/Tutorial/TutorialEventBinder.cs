using System;
using UnityEngine;

public static class TutorialEventBinder
{
    public static void BindBattleTutorial(Action<TutorialTriggerType> startTutorialOneTime, PhaseEventDispatcher eventDispatcher, BanPickStorage banPickStorage)
    {
        eventDispatcher.OnPhasePick += StartPickTutorial;
        eventDispatcher.OnPhaseBan += StartBanTutorial;

        void StartBanTutorial(Team team)
        {
            if (Team.Blue != team) return;
            startTutorialOneTime(TutorialTriggerType.MatchStart);
        }


        void StartPickTutorial(Team team)
        {
            if (Team.Blue != team) return;

            int pickCount = banPickStorage.PickIds.GetTeamCount(Team.Blue);
            switch (pickCount)
            {
                case 0: startTutorialOneTime(TutorialTriggerType.Pick); break;
                case 1: startTutorialOneTime(TutorialTriggerType.MasteryUIEnter); break;
                case 2: startTutorialOneTime(TutorialTriggerType.Swap); break;
            }
        }
    }
}

public enum TutorialTriggerType
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

    public void TriggerIfFirstTime(TutorialTriggerType type)
    {
        if (PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1) return;

        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
        GameObject.Instantiate(_uiTutorial).GetComponent<UI_Tutorial>().StartTutorial(_dialogues);
    }
}