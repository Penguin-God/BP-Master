using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;


public enum TutorialType
{
    Dialogue,
    CheckCardSelect,
}

[System.Serializable]
public class TutorialEntry
{
    [EnumToggleButtons]
    public TutorialType TutorialType;

    public TutorialTriggerType Type;
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
    [SerializeField] TutorialEntry[] entries;
    [SerializeField] GameObject _uiTutorial;

    public void StartTutorialOneTime(TutorialTriggerType type) => new TutorialTrigger(GetTutorialDailogues(type), _uiTutorial).TriggerIfFirstTime(type);

    string[] GetTutorialDailogues(TutorialTriggerType type) => entries.FirstOrDefault(x => x.Type == type)?.Dialogues.ToArray();
}