using UnityEngine;
using UnityEngine.UI;

public class ChampionSelector_UI : MonoBehaviour
{
    [SerializeField] Button nailDownBtn;
    [SerializeField] ChampionView championFocusView;
    
    ChampionSelectPresenter championSelectPresenter = null;
    PhaseManager phaseManager;
    public void Init(ChampionSelectPresenter presenter, PhaseManager pm) // 팀을 아직 안받는 이유는 얘가 팀을 2개를 담당할 때가 있어서
    {
        gameObject.SetActive(true);

        championSelectPresenter = presenter;
        phaseManager = pm;

        nailDownBtn.onClick.AddListener(NailDownChampion);
    }

    Button selectBtn;
    public void SelectChampion(ChampionSO champion, Button button)
    {
        championSelectPresenter.SelectChamp(champion.Id);
        championFocusView.UpdateDisplay(champion);
        selectBtn = button;
    }

    void NailDownChampion()
    {
        championSelectPresenter.NailDownChampion(phaseManager.CurrentFlow);
        phaseManager.SubmitAction(phaseManager.CurrentTurn);
        championFocusView.ClearDisplay();

        ButtonUtil.InActiveButton(selectBtn);
        selectBtn = null;
    }
}