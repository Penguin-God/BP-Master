using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelection : MonoBehaviour
{
    [SerializeField] Button[] stageButtons;

    Func<int> _loadClearStage;

    public void Init(Func<int> loadClearStage, Action<int> onStageSelected)
    {
        _loadClearStage = loadClearStage;

        for (int i = 0; i < stageButtons.Length; i++)
        {
            int index = i; // 클로저(Closure) 캡처 문제 방지
            stageButtons[i].onClick.AddListener(() => onStageSelected?.Invoke(index));
        }

        RefreshUI();
    }

    public void RefreshUI()
    {
        var buttonInteractables = Enumerable.Range(0, stageButtons.Length).Select(i => i <= _loadClearStage()).ToList();

        for (int i = 0; i < stageButtons.Length; i++)
            stageButtons[i].interactable = buttonInteractables[i];
    }
}