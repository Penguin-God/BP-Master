using UnityEngine;
using System.Linq;

[System.Serializable]
public class TutorialEntry
{
    public int ScheduleIndex;

    [TextArea(3, 5)]
    public string[] Dialogues;
}

[CreateAssetMenu(fileName = "TutorialData", menuName = "Data/TutorialData")]
public class TutorialDataSO : ScriptableObject
{
    [SerializeField] TutorialEntry[] entries;
    public TutorialPresenter CreatePresenter() => new TutorialPresenter(entries.ToDictionary(x => x.ScheduleIndex, x => x.Dialogues));
}