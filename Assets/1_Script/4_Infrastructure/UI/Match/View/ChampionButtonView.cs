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
    public IEnumerable<Button> Buttons => buttons;

    UnityAction<ChampionIdentify> clickEvent;

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
        buttons = new ChampionButtonCreator().DrawChampionButtons(content, Ids.Select(x => championManager.GetChampionData(x)), championBtnPrefab);
        if(clickEvent != null)
            ApplyEvnet(clickEvent);
    }

    public Button GetButton(int id) => buttons.First(x => x.GetComponent<ChampionIdentify>().Id == id);
    public void InActiveButton(int id)
    {
        ButtonUtil.InActiveButton(GetButton(id));
        GetButton(id).GetComponentInChildren<TextMeshProUGUI>().color = new Color32(60, 60, 60, 255);
    }

    public void AddEvent(UnityAction<ChampionIdentify> action)
    {
        clickEvent += action;
        ApplyEvnet(action);
    }

    void ApplyEvnet(UnityAction<ChampionIdentify> action)
    {
        print(action);
        foreach (var btn in buttons)
            btn.onClick.AddListener(() => action(btn.GetComponent<ChampionIdentify>()));
    }

    public void InActiveButtons(IEnumerable<int> selectableIds)
    {
        foreach (var btn in buttons.Where(x => selectableIds.Contains(x.GetComponent<ChampionIdentify>().Id) == false))
            InActiveButton(btn.GetComponent<ChampionIdentify>().Id);
    }

    readonly ChampionStatSorter championStatSorter = new ChampionStatSorter();
    IEnumerable<int> GetTabIds(IEnumerable<Champion> allChampions, ChampionSortType sortType) => sortType switch
    {
        ChampionSortType.Default => championManager.AllId,
        ChampionSortType.Attack => championStatSorter.SortByStat(allChampions, StatType.Attack).Select(x => x.Id),
        ChampionSortType.Defense => championStatSorter.SortByStat(allChampions, StatType.Defense).Select(x => x.Id),
        ChampionSortType.Speed => championStatSorter.SortByStat(allChampions, StatType.Speed).Select(x => x.Id),
        _ => throw new System.NotImplementedException(),
    };
}

public class ChampionButtonCreator
{
    public IEnumerable<Button> DrawChampionButtons(Transform parent, IEnumerable<ChampionSO> champions, GameObject btnPrefab)
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