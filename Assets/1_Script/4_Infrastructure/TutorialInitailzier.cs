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

public enum TutorialType
{
    MatchStart,
    SecondSetEnter,
    MasteryUIEnter
}

public class TutorialTrigger
{
    readonly System.Action<TutorialType> _showTutorialUI;

    public TutorialTrigger(System.Action<TutorialType> showTutorialUI) => _showTutorialUI = showTutorialUI;

    public void TriggerIfFirstTime(TutorialType type)
    {
        if (PlayerPrefs.GetInt($"Tutorial_{type}", 0) == 1) return;

        PlayerPrefs.SetInt($"Tutorial_{type}", 1);
        PlayerPrefs.Save();
        _showTutorialUI(type);
    }
}