using UnityEngine;
using System.Linq;

[System.Serializable]
public class TutorialEntry
{
    public TutorialType Type;
    public string[] Dialogues;
}

[CreateAssetMenu(fileName = "TutorialTriggerSO", menuName = "Data/TutorialTriggerSO")]
public class TutorialTriggerSO : ScriptableObject
{
    [SerializeField] TutorialEntry[] entries;
    [SerializeField] GameObject _uiTutorial;

    public void StartTutorialOneTime(TutorialType type) => new TutorialTrigger(GetTutorialDailogues(type), _uiTutorial).TriggerIfFirstTime(type);

    string[] GetTutorialDailogues(TutorialType type) => entries.FirstOrDefault(x => x.Type == type)?.Dialogues.ToArray();
}