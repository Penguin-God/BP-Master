using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] Button slotButton;

    ChampionRepository championManager;
    Champion traickingTarget;
    ChampionView championFocusView;

    void Start() => slotButton.onClick.AddListener(DrawTarget);

    public void Init(ChampionView championFocusView, ChampionRepository championManager)
    {
        this.championFocusView = championFocusView;
        this.championManager = championManager;
    }

    public void UpdateChampion(Champion target)
    {
        traickingTarget = target;
        championView.UpdateChampion(target);
    }

    void DrawTarget() => championFocusView.UpdateDisplay(traickingTarget, championManager.GetChampionData(traickingTarget.Id).TraitData.CreateUI_Data());
}
