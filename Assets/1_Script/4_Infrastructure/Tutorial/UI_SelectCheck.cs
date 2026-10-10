using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectCheck : MonoBehaviour
{
    [SerializeField] Button selectBtn;
    [SerializeField] ChampionSelector_UI championSelector;
    [SerializeField] TutorialTriggerSO tutorialSO;
    [SerializeField] UI_Tutorial ui_Tutorial;

    public Action CreateTutorialAction(TutorialTriggerType type)
    {
        var info = tutorialSO.GetTutorialInfo(type);
        if (info.TutorialType == TutorialType.CheckCardSelect) return () => Do(info);
        else return () => tutorialSO.CreateTutorial(type);
    }

    void Do(Tutorial_Info info)
    {
        var selector = new CheckSelectTutorial(info.AnswerCards.Select(x => x.Id), Success, () => CreateUI().StartTutorial(info.FailDialogues));
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.gameObject.SetActive(true);
        championSelector.ActiveSelectButton(false);
        selectBtn.onClick.AddListener(() => selector.Select(championSelector.SelectId));
        CreateUI().StartTutorial(info.Dialogues);

        void Success()
        {
            CreateUI().StartTutorial(info.SuccessDialogues);
            championSelector.ActiveSelectButton(true);
            selectBtn.gameObject.SetActive(false);
            selectBtn.onClick.RemoveAllListeners();
            championSelector.NailDownChampion();
        }
    }

    UI_Tutorial CreateUI() => Instantiate(ui_Tutorial);
}


public static class BattleTutorialEventBinder
{
    public static void BindBattleTutorial(Func<TutorialTriggerType, Action> createTutorial, PhaseEventDispatcher eventDispatcher, BanPickStorage banPickStorage)
    {
        eventDispatcher.OnPhasePick += StartPickTutorial;
        eventDispatcher.OnPhaseBan += StartBanTutorial;

        void StartBanTutorial(Team team)
        {
            if (Team.Blue != team) return;
            createTutorial(TutorialTriggerType.MatchStart)();
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