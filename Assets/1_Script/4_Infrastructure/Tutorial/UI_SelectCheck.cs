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
        if (info.TutorialType == TutorialType.CheckCardSelect) return () => TutorialActor.StartIfFirstTime(type, () => Do(info));
        else return () => tutorialSO.StartTutorialOneTime(type);
    }

    public Action CreateConditionTutorialAction(TutorialTriggerType type)
    {
        var info = tutorialSO.GetTutorialInfo(type);
        if (info.TutorialType == TutorialType.CheckCardSelect) return () => TutorialActor.StartIfFirstTime(type, () => DoByCondition(info));
        else return () => tutorialSO.StartTutorialOneTime(type);
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

    void DoByCondition(Tutorial_Info info)
    {
        var condition = TutorialQuizConditionFactory.Create(info.CreateQuizConditionData());
        var selector = new CheckSelectTutorial(condition, Success, () => CreateUI().StartTutorial(info.FailDialogues));
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.gameObject.SetActive(true);
        championSelector.ActiveSelectButton(false);
        selectBtn.onClick.AddListener(() =>
        {
            var champion = ChampionDataLoder.GetChampionData(championSelector.SelectId).CreateChampion();
            selector.SelectChampion(champion);
        });
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