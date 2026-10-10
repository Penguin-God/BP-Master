using System;
using UnityEngine;

public enum TutorialTriggerType
{
    MatchStart,
    MasteryUIEnter,
    Pick,
    Swap,
}

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