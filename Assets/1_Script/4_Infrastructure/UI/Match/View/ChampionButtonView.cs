using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChampionButtonView : MonoBehaviour
{
    [SerializeField] ChampionRepository championManager;
    [SerializeField] GameObject championBtn;
    [SerializeField] Transform content;

    IEnumerable<Button> buttons;
    public IEnumerable<Button> Buttons => buttons;

    public void CreateButtons() => buttons = new ChampionButtonCreator().DrawChampionButtons(content, championManager.AllChampion, championBtn);

    public Button GetButton(int id) => buttons.First(x => x.GetComponent<ChampionIdentify>().Id == id);
    public void InActiveButton(int id)
    {
        ButtonUtil.InActiveButton(GetButton(id));
        GetButton(id).GetComponentInChildren<TextMeshProUGUI>().color = new Color32(60, 60, 60, 255);
    }
    public void AddEvent(UnityAction<ChampionIdentify> action)
    {
        foreach (var btn in buttons)
            btn.onClick.AddListener(() => action(btn.GetComponent<ChampionIdentify>()));
    }

    public void InActiveButtons(IEnumerable<int> selectableIds)
    {
        foreach (var btn in buttons.Where(x => selectableIds.Contains(x.GetComponent<ChampionIdentify>().Id) == false))
            InActiveButton(btn.GetComponent<ChampionIdentify>().Id);
    }
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