using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChampionDrawer : MonoBehaviour
{
    [SerializeField] ChampionManager championManager;
    [SerializeField] GameObject championBtn;

    public void DrawChampionButtons(UnityAction<ChampionSO> onclick)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var data in championManager.AllChampion)
        {
            var btn = Instantiate(championBtn, transform).GetComponent<Button>();
            btn.GetComponentInChildren<TextMeshProUGUI>().text = data.ChampionName;
            btn.onClick.AddListener(() => onclick(data));
        }
    }
}
