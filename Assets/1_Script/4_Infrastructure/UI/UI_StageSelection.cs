using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelection : MonoBehaviour
{
    [SerializeField] Button[] stageButtons;

    StageProgressPresenter _presenter;

    public void Init(StageProgressPresenter presenter, Action<int> onStageSelected)
    {
        _presenter = presenter;

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // 클로저(Closure) 캡처 문제 방지
            stageButtons[i].onClick.AddListener(() => onStageSelected?.Invoke(index));
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        var states = _presenter.GetButtonStates(stageButtons.Length);

        for (int i = 0; i < stageButtons.Length; i++)
        {
            stageButtons[i].interactable = states[i];
        }
    }
}