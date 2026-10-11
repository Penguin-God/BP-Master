using System;

public enum TutorialTriggerType
{
    MatchStart,
    MasteryUIEnter,
    FirstPick,
    SecondPick,
    ThreedPick,
    Ban,
    Swap,
}

public static class BattleTutorialEventBinder
{
    public static void BindBattleTutorial(Func<TutorialTriggerType, Action> createTutorial, PhaseEventDispatcher eventDispatcher, BanPickStorage banPickStorage)
    {
        eventDispatcher.OnPhasePick += StartPickTutorial;
        eventDispatcher.OnPhaseBan += StartBanTutorial;

        void StartBanTutorial(Team team)
        {
            UnityEngine.Debug.Log(team);
            createTutorial(TutorialTriggerType.Ban)();
        }


        void StartPickTutorial(Team team)
        {
            if (Team.Blue != team) return;

            int pickCount = banPickStorage.PickIds.GetTeamCount(Team.Blue);
            switch (pickCount)
            {
                case 0: createTutorial(TutorialTriggerType.FirstPick)(); break;
                case 1: createTutorial(TutorialTriggerType.SecondPick)(); break;
                case 2: createTutorial(TutorialTriggerType.ThreedPick)(); break;
            }
        }
    }
}