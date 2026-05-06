using UnityEngine;

public class UI_Leaderboard : MonoBehaviour
{
    [SerializeField] UI_LeaderboardItem itemPrefab;
    [SerializeField] Transform gridContainer;

    LeaderboardPresenter _presenter;

    public void Init(LeaderboardPresenter presenter)
    {
        _presenter = presenter;
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in gridContainer) Destroy(child.gameObject);

        foreach (var model in _presenter.GetDisplayData())
        {
            var inst = Instantiate(itemPrefab, gridContainer);
            inst.Bind(model);
        }
    }
}