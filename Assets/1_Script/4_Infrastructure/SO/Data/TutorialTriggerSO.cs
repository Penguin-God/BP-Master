using UnityEngine;
using System.Linq;

[System.Serializable]
public class TutorialEntry
{
    public TutorialType Type;
    [TextArea(3, 5)] public string[] Dialogues;
}

[CreateAssetMenu(fileName = "TutorialTriggerSO", menuName = "BP Master/TutorialTriggerSO")]
public class TutorialTriggerSO : ScriptableObject, ITutorialViewer
{
    [SerializeField] TutorialEntry[] entries;

    UI_Tutorial _uiTutorial;

    // 구체적인 UI 클래스를 직접 주입받습니다.
    public void Inject(UI_Tutorial uiTutorial) => _uiTutorial = uiTutorial;

    public TutorialTriggerUseCase CreateUseCase()
        => new TutorialTriggerUseCase(new PlayerPrefsTutorialStorage(), this);

    // ITutorialViewer 구현부
    public void Show(TutorialType type)
    {
        var dialogues = entries.FirstOrDefault(x => x.Type == type)?.Dialogues;

        if (dialogues != null && dialogues.Length > 0 && _uiTutorial != null)
        {
            _uiTutorial.StartTutorial(dialogues);
        }
    }
}