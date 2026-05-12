using UnityEngine;
using System.Linq;

[System.Serializable]
public class TutorialEntry
{
    public TutorialType Type;
    public string[] Dialogues;
}

[CreateAssetMenu(fileName = "TutorialTriggerSO", menuName = "Data/TutorialTriggerSO")]
public class TutorialTriggerSO : ScriptableObject, ITutorialViewer
{
    [SerializeField] TutorialEntry[] entries;

    UI_Tutorial _uiTutorial;

    public void Inject(UI_Tutorial uiTutorial) => _uiTutorial = uiTutorial;

    public void StartTutorial(TutorialType type) => new TutorialTriggerUseCase(new PlayerPrefsTutorialStorage(), this).TriggerIfFirstTime(type);

    public void Show(TutorialType type)
    {
        var dialogues = entries.FirstOrDefault(x => x.Type == type)?.Dialogues;

        if (dialogues != null && dialogues.Length > 0 && _uiTutorial != null)
        {
            _uiTutorial.StartTutorial(dialogues);
        }
    }
}