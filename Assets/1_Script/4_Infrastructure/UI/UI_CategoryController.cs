using UnityEngine;
using UnityEngine.UI;

public class UI_CategoryController : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] Button stageBtn;
    [SerializeField] Button masteryBtn;

    [Header("패널")]
    [SerializeField] GameObject stagePanel;
    [SerializeField] GameObject masteryPanel;

    [SerializeField] TutorialTriggerSO tutorialTrigger;

    void Awake()
    {
        stageBtn.onClick.AddListener(ShowStage);
        masteryBtn.onClick.AddListener(ShowMastery);
    }

    void Start()
    {
        ShowStage();
    }

    void ShowStage()
    {
        stagePanel.SetActive(true);
        masteryPanel.SetActive(false);
    }

    void ShowMastery()
    {
        stagePanel.SetActive(false);
        masteryPanel.SetActive(true);
        tutorialTrigger.StartTutorialOneTime(TutorialType.MasteryUIEnter);
    }
}