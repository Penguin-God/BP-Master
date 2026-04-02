using UnityEngine;
using UnityEngine.UI;

public class UI_CategoryController : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] Button leagueBtn;
    [SerializeField] Button masteryBtn;

    [Header("패널")]
    [SerializeField] GameObject leaguePanel;
    [SerializeField] GameObject masteryPanel;

    void Awake()
    {
        leagueBtn.onClick.AddListener(ShowSchedule);
        masteryBtn.onClick.AddListener(ShowMastery);
    }

    void Start()
    {
        // 로비 진입 시 기본적으로 리그(스케줄) 화면을 보여줍니다.
        ShowSchedule();
    }

    void ShowSchedule()
    {
        leaguePanel.SetActive(true);
        masteryPanel.SetActive(false);
    }

    void ShowMastery()
    {
        leaguePanel.SetActive(false);
        masteryPanel.SetActive(true);
    }
}