using UnityEngine;
using UnityEngine.UI;

public class UI_CategoryController : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] Button stageBtn;
    [SerializeField] Button deckBtn;

    [Header("패널")]
    [SerializeField] GameObject stagePanel;
    [SerializeField] GameObject deckPanel;

    [SerializeField] TutorialTriggerSO tutorialTrigger;

    void Awake()
    {
        stageBtn.onClick.AddListener(ShowStage);
        deckBtn.onClick.AddListener(ShowMastery);
    }

    void Start()
    {
        ShowStage();
    }

    void ShowStage()
    {
        stagePanel.SetActive(true);
        deckPanel.SetActive(false);
    }

    void ShowMastery()
    {
        stagePanel.SetActive(false);
        deckPanel.SetActive(true);
        tutorialTrigger.StartTutorialOneTime(TutorialType.MasteryUIEnter);
    }
}