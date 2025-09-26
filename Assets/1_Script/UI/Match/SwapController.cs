using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapController : MonoBehaviour
{
    [SerializeField] Button swapDoneBtn;
    [SerializeField] Button[] blueSlotButtons;
    [SerializeField] Button[] redSlotButtons;

    PhaseManager phaseManager;
    GameBanPickStorage banPickStorage;

    // 팀별 선택 인덱스 임시 보관
    readonly Dictionary<Team, List<int>> selected = new Dictionary<Team, List<int>>
    {
        { Team.Blue, new List<int>(2) },
        { Team.Red,  new List<int>(2) },
    };

    public void Init(Team team)
    {
        gameObject.SetActive(true);
        swapDoneBtn.onClick.AddListener(() => SwapDone(team));

        Button[] buttons  = null;
        if (team == Team.Blue) buttons = blueSlotButtons;
        else if(team == Team.Red) buttons = redSlotButtons;

        int index = 0;
        foreach (var item in buttons)
        {
            int newIndex = index;
            item.onClick.AddListener(() => OnSlotClicked(Team.Blue, newIndex));
            index++;
        }
    }

    public void Inject(PhaseManager phaseManager, GameBanPickStorage banPickStorage)
    {
        this.phaseManager = phaseManager;
        this.banPickStorage = banPickStorage;
    }


    void OnSlotClicked(Team team, int index)
    {
        if (phaseManager.CurrentFlow.Phase != GamePhase.Swap) return;

        var bag = selected[team];

        // 동일 인덱스 중복 누름 방지
        if (bag.Count == 1 && bag[0] == index) return;

        bag.Add(index);
        if (bag.Count == 2)
        {
            banPickStorage.Swap(team, bag[0], bag[1]);
            bag.Clear();
            print("AA");
        }
    }

    void SwapDone(Team team)
    {
        if (phaseManager.CurrentFlow.Phase == GamePhase.Swap)
        {
            phaseManager.SubmitAction(team);
            gameObject.SetActive(false);
        }
    }
}
