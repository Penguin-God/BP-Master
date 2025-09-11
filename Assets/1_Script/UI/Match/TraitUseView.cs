using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUseView : MonoBehaviour
{
    [SerializeField] Button[] blueChamps;
    [SerializeField] Button[] redChamps;
    Dictionary<Team, Button[]> buttons = new();

    TraitUsePresenter presenter;
    
    public void Init(TraitUsePresenter presenter)
    {
        gameObject.SetActive(true);
        this.presenter = presenter;
    }

    void Start()
    {
        buttons.Add(Team.Blue, blueChamps);
        buttons.Add(Team.Red, redChamps);

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
    }

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
        var result = presenter.ClickChampion(new ChampionSlot(buttonTeam, index));
        switch (result)
        {
            case TraitClickResult.Faild:
                print("선택실패");
                break;
            case TraitClickResult.Select:
                print(buttonTeam + " select");
                break;
            case TraitClickResult.Use:
                break;
        }
    }
}
