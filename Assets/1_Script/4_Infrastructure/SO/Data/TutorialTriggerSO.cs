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
    [SerializeField] GameObject _uiTutorial;

    public void StartTutorialOneTime(TutorialType type) => new TutorialTriggerUseCase(new PlayerPrefsTutorialStorage(), this).TriggerIfFirstTime(type);

    public void Show(TutorialType type)
    {
        var dialogues = entries.FirstOrDefault(x => x.Type == type)?.Dialogues;

        if (dialogues?.Length > 0)
        {
            var clone = Instantiate(_uiTutorial);
            clone.GetComponent<UI_Tutorial>().StartTutorial(dialogues);
        }
    }
}