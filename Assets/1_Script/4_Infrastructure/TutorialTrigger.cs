using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("데이터 및 프리팹")]
    [SerializeField] TutorialDataSO tutorialDataSO;
    [SerializeField] UI_Tutorial uiTutorialPrefab;

    public void PlayTutorial()
    {
        IScheduleStorage storage = new PlayerPrefsScheduleStorage();
        int currentIndex = storage.LoadIndex();
        TutorialPresenter presenter = tutorialDataSO.CreatePresenter();
        var ui = Instantiate(uiTutorialPrefab);
        ui.StartTutorial(presenter, currentIndex);
    }
}