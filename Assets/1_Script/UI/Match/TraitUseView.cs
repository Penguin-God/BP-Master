using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUseView : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;
    Dictionary<Team, Button[]> buttons = new();

    TraitPresenter presenter;
    Team currentTeam;
    PhaseManager phaseManager;
    public void Init(TraitPresenter presenter, PhaseManager phaseManager)
    {
        gameObject.SetActive(true);
        this.presenter = presenter;   
        this.phaseManager = phaseManager;
    }

    void Start()
    {
        buttons.Add(Team.Blue, blueChamps);
        buttons.Add(Team.Red, redChamps);

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
    }

    public void ChangeTeam(Team team) => currentTeam = team;

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnButtonClicked(buttonTeam, index));
        }
    }


    void OnButtonClicked(Team buttonTeam, int index)
    {
        if (presenter.IsSelected == false && currentTeam != buttonTeam) return;

        print(index);
        if(presenter.IsSelected)
        {
            if (presenter.UseTrait(index))
                phaseManager.SubmitAction(currentTeam);
        }
        else presenter.SelectTrait(currentTeam, index);
    }
}
