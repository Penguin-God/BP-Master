using System.Collections;
using TMPro;
using UnityEngine;

public class StatChangeView : MonoBehaviour
{
    [SerializeField] StatView statView;
    [SerializeField] TextMeshProUGUI attChangeText;
    [SerializeField] TextMeshProUGUI defChangeText;
    [SerializeField] TextMeshProUGUI speedChangeText;

    StatChangePresenter statChangePresenter = new StatChangePresenter(Color.green, Color.red);

    void Start() => InActiveTexts();

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
        yield return new WaitForSeconds(1.5f);

        statView.UpdateStat(afterStat);
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
