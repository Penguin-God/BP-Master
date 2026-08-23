using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ChampionSortType
{
    Default,
    Attack,
    Defense,
    Speed
}

public class ChampionButtonView : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] GameObject championBtnPrefab;
    [SerializeField] Transform content;

    [Header("Tab Buttons")]
    [SerializeField] Button defaultTabBtn;
    [SerializeField] Button attackTabBtn;
    [SerializeField] Button defenseTabBtn;
    [SerializeField] Button speedTabBtn;

    IEnumerable<Button> buttons;
    UnityAction<ChampionIdentify> clickEvent;
    ChampionButtonStatePresenter _presenter;

    void Start()
    {
        var champions = championManager.AllChampion.Select(x => x.CreateChampion());
        defaultTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Default)));
        attackTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Attack)));
        defenseTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Defense)));
        speedTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Speed)));
    }

    public void CreateButtons() => CreateButtons(championManager.AllId);

    void CreateButtons(IEnumerable<int> Ids)
    {
        foreach (Transform child in content) Destroy(child.gameObject);

        buttons = ChampionButtonCreator.CreateChampionButtons(content, Ids.Select(x => championManager.GetChampionData(x)), championBtnPrefab);

        if (clickEvent != null)
            ApplyEvent(clickEvent);

        ApplyStates();
    }

    public void Init(ChampionButtonStatePresenter presenter)
    {
        _presenter = presenter;
        ApplyStates();
    }

    void ApplyStates()
    {
        if (buttons == null || _presenter == null) return;

        foreach (var btn in buttons)
        {
            var identify = btn.GetComponent<ChampionIdentify>();
            var state = _presenter.GetState(identify.Id); // 단일 모델 반환

            var text = btn.GetComponentInChildren<TextMeshProUGUI>();
            text.text = state.Name;
            text.color = state.TextColor;
            ButtonUtil.ChangeButtonColor(btn, state.ButtonColor);

            if (state.IsEnabled == false) ButtonUtil.InActiveButton(btn);
            else btn.interactable = true;
        }
    }

    public void AddEvent(UnityAction<ChampionIdentify> action)
    {
        clickEvent += action;
        ApplyEvent(action);
    }

    void ApplyEvent(UnityAction<ChampionIdentify> action)
    {
        foreach (var btn in buttons)
            btn.onClick.AddListener(() => action(btn.GetComponent<ChampionIdentify>()));
    }

    IEnumerable<int> GetTabIds(IEnumerable<Champion> allChampions, ChampionSortType sortType) => sortType switch
    {
        ChampionSortType.Default => championManager.AllId,
        ChampionSortType.Attack => SortByStat(allChampions, StatType.Attack),
        ChampionSortType.Defense => SortByStat(allChampions, StatType.Defense),
        ChampionSortType.Speed => SortByStat(allChampions, StatType.Speed),
        _ => throw new System.NotImplementedException(),
    };

    IEnumerable<int> SortByStat(IEnumerable<Champion> champions, StatType statType)
        => champions
            .OrderByDescending(c => c.Status.Stat.GetStatValue(statType))
            .Select(x => x.Id);

    public Button GetButton(int id) => buttons.First(x => x.GetComponent<ChampionIdentify>().Id == id);
    public void InActiveButton(int id)
    {
        ButtonUtil.InActiveButton(GetButton(id));
        GetButton(id).GetComponentInChildren<TextMeshProUGUI>().color = new Color32(60, 60, 60, 255);
    }
}

public static class ChampionButtonCreator
{
    public static IEnumerable<Button> CreateChampionButtons(Transform parent, IEnumerable<ChampionSO> champions, GameObject btnPrefab)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);

        var result = new List<Button>();
        foreach (var data in champions)
        {
            var btn = Object.Instantiate(btnPrefab, parent).GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = data.ChampionName;
            btn.GetOrAddComponent<ChampionIdentify>().Id = data.Id;
            result.Add(btn);
        }
        return result;
    }
}