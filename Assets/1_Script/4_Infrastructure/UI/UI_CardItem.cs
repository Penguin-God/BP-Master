using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class UI_CardItem : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Button button;

    public int Id => GetIdentify().Id;
    public ChampionIdentify Identify => GetIdentify();

    ChampionIdentify GetIdentify() => this.GetOrAddComponent<ChampionIdentify>();

    public void DrawCard(ChampionSO championSO)
    {
        GetIdentify().Id = championSO.Id;
        icon.sprite = championSO.Icon;
        nameText.text = championSO.ChampionName;
    }

    public void ApplyState(string name, Color textColor, Color buttonColor, bool isEnabled)
    {
        nameText.text = name;
        nameText.color = textColor;
        ButtonUtil.ChangeButtonColor(button, buttonColor);

        if (isEnabled == false) ButtonUtil.InActiveButton(button);
        else button.interactable = true;
    }

    public void InActive()
    {
        ButtonUtil.InActiveButton(button);
        nameText.color = new Color32(60, 60, 60, 255);
    }

    public void AddClickListener(UnityAction<ChampionIdentify> action)
    {
        button.onClick.AddListener(() => action(Identify));
    }
}