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
    [SerializeField] GameObject championBtnPrefab;
    [SerializeField] Transform content;

    [Header("Tab Buttons")]
    [SerializeField] Button defaultTabBtn;
    [SerializeField] Button attackTabBtn;
    [SerializeField] Button defenseTabBtn;
    [SerializeField] Button speedTabBtn;

    IEnumerable<UI_CardItem> _cards;
    UnityAction<ChampionSO> clickEvent;
    ChampionButtonStatePresenter _presenter;
    IEnumerable<int> _selectableCards;

    public void Init(ChampionButtonStatePresenter presenter, IEnumerable<int> selectableCards)
    {
        _presenter = presenter;
        ApplyStates();

        _selectableCards = selectableCards;
        CreateButtons(_selectableCards);

        var champions = _selectableCards.Select(x => ChampionDataLoder.GetChampionData(x).CreateChampion());
        defaultTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Default)));
        attackTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Attack)));
        defenseTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Defense)));
        speedTabBtn.onClick.AddListener(() => CreateButtons(GetTabIds(champions, ChampionSortType.Speed)));
    }

    void CreateButtons(IEnumerable<int> Ids)
    {
        var cardPrefab = championBtnPrefab.GetComponent<UI_CardItem>();
        _cards = ChampionButtonCreator.CreateCardItems(content, Ids.Select(x => ChampionDataLoder.GetChampionData(x)), cardPrefab);

        if (clickEvent != null) ApplyEvent(clickEvent);

        ApplyStates();
    }

    void ApplyStates()
    {
        if (_cards == null || _presenter == null) return;

        foreach (var card in _cards)
        {
            var state = _presenter.GetState(card.Id);
            card.ApplyState(state.Name, state.TextColor, state.ButtonColor, state.IsEnabled);
        }
    }

    public void AddEvent(UnityAction<ChampionSO> action)
    {
        clickEvent += action;
        ApplyEvent(action);
    }

    void ApplyEvent(UnityAction<ChampionSO> action)
    {
        foreach (var card in _cards)
            card.AddClickListener(action);
    }

    IEnumerable<int> GetTabIds(IEnumerable<Champion> allChampions, ChampionSortType sortType) => sortType switch
    {
        ChampionSortType.Default => _selectableCards,
        ChampionSortType.Attack => SortByStat(allChampions, StatType.Attack),
        ChampionSortType.Defense => SortByStat(allChampions, StatType.Defense),
        ChampionSortType.Speed => SortByStat(allChampions, StatType.Speed),
        _ => throw new System.NotImplementedException(),
    };

    IEnumerable<int> SortByStat(IEnumerable<Champion> champions, StatType statType)
        => champions
            .OrderByDescending(c => c.Status.Stat.GetStatValue(statType))
            .Select(x => x.Id);

    public void InActiveButton(int id)
    {
        var card = _cards.FirstOrDefault(x => x.Id == id);
        if (card == null) return;
        card.InActive();
    }
}

public static class ChampionButtonCreator
{
    public static IEnumerable<Button> CreateChampionButtons(Transform parent, IEnumerable<ChampionSO> champions, GameObject catdPrefab)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);

        var result = new List<Button>();
        foreach (var data in champions)
        {
            var btn = Object.Instantiate(catdPrefab, parent).GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = data.ChampionName;
            btn.GetOrAddComponent<ChampionIdentify>().Id = data.Id;
            result.Add(btn);
        }
        return result;
    }

    public static IEnumerable<UI_CardItem> CreateCardItems(Transform parent, IEnumerable<ChampionSO> champions, UI_CardItem catdPrefab)
    {
        foreach (Transform child in parent)
            Object.Destroy(child.gameObject);

        var result = new List<UI_CardItem>();
        foreach (var data in champions)
        {
            var card = Object.Instantiate(catdPrefab, parent).GetComponent<UI_CardItem>();
            card.DrawCard(data);
            result.Add(card);
        }
        return result;
    }
}