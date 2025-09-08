using UnityEngine;
using UnityEngine.UI;

public class TraitUseView : MonoBehaviour
{
    [SerializeField] Button[] championButtons;
    [SerializeField] Button[] targetButtons;
    
    TraitPresenter presenter;
    Team currentTeam;
    
    public void Init(TraitPresenter presenter)
    {
        this.presenter = presenter;
        SetupChampionButtons();
    }
    public void ChangeTeam(Team team) => currentTeam = team;

    void SetupChampionButtons()
    {
        for (int i = 0; i < championButtons.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            championButtons[i].onClick.AddListener(() => OnChampionButtonClicked(index));
        }
    }

    void OnChampionButtonClicked(int index) => presenter.SelectTrait(currentTeam, index);
}
