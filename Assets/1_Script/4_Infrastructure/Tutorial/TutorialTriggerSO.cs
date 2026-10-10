using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;


public enum TutorialType
{
    Dialogue,
    CheckCardSelect,
}

[System.Serializable]
public class Tutorial_Info
{
    [EnumToggleButtons]
    public TutorialType TutorialType;

    public TutorialTriggerType TriggerType;
    public string[] Dialogues;

    [ShowIf(nameof(IsQuiz))]
    [Title("문제 설정")]
    public ChampionSO[] AnswerCards;

    [ShowIf(nameof(IsQuiz))]
    public string[] SuccessDialogues;

    [ShowIf(nameof(IsQuiz))]
    public string[] FailDialogues;

    bool IsQuiz => TutorialType == TutorialType.CheckCardSelect;
}

[CreateAssetMenu(fileName = "TutorialTriggerSO", menuName = "Data/TutorialTriggerSO")]
public class TutorialTriggerSO : ScriptableObject
{
    [SerializeField] Tutorial_Info[] entries;
    [SerializeField] GameObject _uiTutorial;

    public void StartTutorialOneTime(TutorialTriggerType type) => new TutorialTrigger(GetTutorialDailogues(type), _uiTutorial).TriggerIfFirstTime(type);

    string[] GetTutorialDailogues(TutorialTriggerType type) => entries.FirstOrDefault(x => x.TriggerType == type)?.Dialogues;

    Tutorial_Info GetTutorialInfo(TutorialTriggerType type) => entries.FirstOrDefault(x => x.TriggerType == type);

    public void TutorialOneTime(TutorialTriggerType type)
    {
        var tutoInfo = GetTutorialInfo(type);
        new TutorialTrigger(tutoInfo.Dialogues, _uiTutorial).TriggerIfFirstTime(type);

        if (tutoInfo.TutorialType == TutorialType.CheckCardSelect)
        {

        }
    }
}