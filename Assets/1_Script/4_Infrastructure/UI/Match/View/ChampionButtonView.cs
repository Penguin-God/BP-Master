using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChampionButtonView : MonoBehaviour
{
    [SerializeField] UI_CardItem cardPrefab;
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

    void Start() => ObjectUtil.DestoryAllChild(content);

    public void Init(ChampionButtonStatePresenter presenter, IEnumerable<int> selectableCards)
    {
        _presenter = presenter;
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
        _cards = ChampionButtonCreator.CreateCardItems(content, Ids.Select(x => ChampionDataLoder.GetChampionData(x)), cardPrefab);

        if (clickEvent != null) ApplyEvent(clickEvent);

        foreach (var card in _cards)
            card.DrawModel(_presenter.GetCardModel(card.Id));
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

    IEnumerable<int> GetTabIds(IEnumerable<Champion> allChampions, ChampionSortType sortType) => ChampionSortHelper.GetTabIds(allChampions, sortType, _selectableCards);

    IEnumerable<int> SortByStat(IEnumerable<Champion> champions, StatType statType) => ChampionSortHelper.SortByStat(champions, statType);

    public void InActiveButton(int id)
    {
        var card = _cards.FirstOrDefault(x => x.Id == id);
        if (card == null) return;
        card.InActive();
    }
}

public static class ChampionButtonCreator
{
    public static IEnumerable<UI_CardItem> CreateCardItems(Transform parent, IEnumerable<ChampionSO> champions, UI_CardItem cardPrefab)
        => ObjectUtil.SpawnItems(parent, cardPrefab, champions, (card, data) => card.DrawCard(data));
}
