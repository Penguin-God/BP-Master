using System.Collections.Generic;
using System.Linq;
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

        buttons.Add(Team.Blue, blueChamps);
        buttons.Add(Team.Red, redChamps);

        SetupChampionButtons(blueChamps, Team.Blue);
        SetupChampionButtons(redChamps, Team.Red);
        InActiveAllBtns();
    }

    void SetupChampionButtons(Button[] btns, Team buttonTeam)
    {
        for (int i = 0; i < btns.Length; i++)
        {
            int index = i; // 클로저 캡처 방지
            btns[i].onClick.AddListener(() => OnButtonClicked(buttonTeam, index));
        }
    }

    void InActiveAllBtns()
    {
        foreach (Button item in buttons.Values.SelectMany(x => x))
            item.enabled = false;
    }

    public void UpdateTrait(Team team) // ActiveButtons 발동 순간이 애매함
    {
        presenter.ChangeTeam(team);
        ActiveButtons();
    }

    void ActiveButtons()
    {
        InActiveAllBtns();
        foreach (var slot in presenter.GetClickableSlots())
            buttons[slot.Team][slot.Index].enabled = true;
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
                ActiveButtons();
                break;
            case TraitClickResult.Use:
                print(buttonTeam + " use");
                ActiveButtons();
                break;
        }
    }
}
