using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

public enum TutorialType
{
    Dialogue,
    CheckCardSelect,
}

[Serializable]
public class Tutorial_Info
{
    [Title("===튜토리얼 정보 시작===")]
    [EnumToggleButtons]
    public TutorialType TutorialType;

    public TutorialTriggerType TriggerType;
    public string[] Dialogues;

    [ShowIf(nameof(IsQuiz))]
    [Title("문제 설정")]

    [ShowIf(nameof(IsQuiz))]
    [EnumToggleButtons]
    public TutorialQuizType QuizType;

    [ShowIf(nameof(IsSpeedQuiz))]
    public int MinSpeed;

    [ShowIf(nameof(IsQuiz))]
    public string[] SuccessDialogues;

    [ShowIf(nameof(IsQuiz))]
    public string[] FailDialogues;

    bool IsQuiz => TutorialType == TutorialType.CheckCardSelect;
    bool IsSpeedQuiz => IsQuiz && QuizType == TutorialQuizType.SpeedAtLeast;

    public TutorialQuizConditionData CreateQuizConditionData() => new TutorialQuizConditionData(QuizType, MinSpeed);
}
[CreateAssetMenu(fileName = "TutorialTriggerSO", menuName = "Data/TutorialTriggerSO")]
public class TutorialTriggerSO : ScriptableObject
{
    [SerializeField] Tutorial_Info[] entries;
    [SerializeField] GameObject _uiTutorial;

    public void StartTutorialOneTime(TutorialTriggerType type) => TutorialActor.StartIfFirstTime(type, CreateTutorial(type));
    public Tutorial_Info GetTutorialInfo(TutorialTriggerType type) => entries.FirstOrDefault(x => x.TriggerType == type);

    Action CreateTutorial(TutorialTriggerType type)
    {
        var info = GetTutorialInfo(type);
        return () => GameObject.Instantiate(_uiTutorial).GetComponent<UI_Tutorial>().StartTutorial(info.Dialogues);
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