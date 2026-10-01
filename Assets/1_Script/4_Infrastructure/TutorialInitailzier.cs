using Match;
using UnityEngine;

public class TutorialInitailzier : MonoBehaviour
{
    [SerializeField] TutorialTriggerSO tutorialTriggerSO;

    void Start()
    {
                
    }

    void MatchTutorial()
    {
        if (MatchContext.MatchState.TotalWins == 0) tutorialTriggerSO.StartTutorialOneTime(TutorialType.MatchStart);
        else tutorialTriggerSO.StartTutorialOneTime(TutorialType.SecondSetEnter);
    }
}

public class PlayerPrefsTutorialStorage : ITutorialStorage
{
    public bool HasSeen(TutorialType type) => PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1;

    public void MarkAsSeen(TutorialType type)
    {
        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
    }
}

public enum TutorialType
{
    MatchStart,
    SecondSetEnter,
    MasteryUIEnter
}

public class TutorialTriggerUseCase
{
    readonly ITutorialStorage _storage;
    readonly ITutorialViewer _viewer;

    public TutorialTriggerUseCase(ITutorialStorage storage, ITutorialViewer viewer)
    {
        _storage = storage;
        _viewer = viewer;
    }

    public void TriggerIfFirstTime(TutorialType type)
    {
        if (_storage.HasSeen(type)) return;

        _storage.MarkAsSeen(type);
        _viewer.Show(type);
    }
}

public interface ITutorialStorage
{
    bool HasSeen(TutorialType type);
    void MarkAsSeen(TutorialType type);
}

public interface ITutorialViewer
{
    void Show(TutorialType type);
}