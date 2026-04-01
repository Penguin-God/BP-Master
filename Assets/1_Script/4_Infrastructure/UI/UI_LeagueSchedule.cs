using UnityEngine;
using UnityEngine.UI;

public class UI_LeagueSchedule : MonoBehaviour
{
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    [SerializeField] UI_ScheduleMatchItem itemPrefab;
    [SerializeField] Transform itemContainer;

    SchedulePaginationPresenter presenter;

    public void Init(SchedulePaginationPresenter presenter)
    {
        this.presenter = presenter;

        nextButton.onClick.AddListener(OnClickNext);
        prevButton.onClick.AddListener(OnClickPrev);

        RefreshUI();
    }

    void OnClickNext()
    {
        presenter.NextPage();
        RefreshUI();
    }

    void OnClickPrev()
    {
        presenter.PrevPage();
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in itemContainer) Destroy(child.gameObject);

        foreach (var data in presenter.GetCurrentPageData())
            Instantiate(itemPrefab, itemContainer).Bind(data);
    }
}