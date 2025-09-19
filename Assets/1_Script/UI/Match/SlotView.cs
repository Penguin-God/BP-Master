using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] ChampionView championView;
    [SerializeField] TextMeshProUGUI attChangeText;
    [SerializeField] TextMeshProUGUI defChangeText;
    [SerializeField] TextMeshProUGUI speedChangeText;
    [SerializeField] Button slotButton;

    ChampionManagerMono championManager;
    // SlotStorage<Champion> pickStorage;
    Champion traickingTarget;
    ChampionView traickingView;

    StatChangePresenter statChangePresenter = new StatChangePresenter(Color.green, Color.red);

    void Start()
    {
        InActiveTexts();
        slotButton.onClick.AddListener(DrawTarget);
    }

    public void Init(ChampionView view, ChampionManagerMono championManager)
    {
        traickingView = view;
        this.championManager = championManager;
    }

    public void UpdateChampion(Champion target)
    {
        traickingTarget = target;
        championView.UpdateChampion(target);
    }

    void DrawTarget() => traickingView.UpdateDisplay(traickingTarget, championManager.GetChampionData(traickingTarget.Id).TraitData.CreateUI_Data());

    public void ChangeStat(StatChangeData changeData)
    {
        var changeViewModel = statChangePresenter.CreateViewModel(changeData);

        ViewStatChange(changeViewModel.Attack, attChangeText);
        ViewStatChange(changeViewModel.Defense, defChangeText);
        ViewStatChange(changeViewModel.Speed, speedChangeText);

        StartCoroutine(ApplyStatChangeAfterDelay(changeData.After));
    }

    IEnumerator ApplyStatChangeAfterDelay(ChampionStatData afterStat)
    {
        yield return new WaitForSeconds(2f);

        championView.UpdateStat(afterStat);
        InActiveTexts();
    }

    void InActiveTexts()
    {
        attChangeText.gameObject.SetActive(false);
        defChangeText.gameObject.SetActive(false);
        speedChangeText.gameObject.SetActive(false);
    }

    void ViewStatChange(StatDeltaViewModel deltaViewModel, TextMeshProUGUI text)
    {
        if (deltaViewModel.IsChange == false) return;

        text.gameObject.SetActive(true);
        text.color = deltaViewModel.DeltaTextColor;
        text.text = deltaViewModel.DeltaText;
    }
}
