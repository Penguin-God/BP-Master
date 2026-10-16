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

    ChampionSO _cardData;
    public int Id => _cardData.Id;

    ChampionIdentify GetIdentify() => this.GetOrAddComponent<ChampionIdentify>();

    public void DrawCard(ChampionSO championSO)
    {
        _cardData = championSO;
        GetIdentify().Id = championSO.Id;
        icon.sprite = _cardData.Icon;
        nameText.text = _cardData.ChampionName;
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

    public void AddClickListener(UnityAction<ChampionSO> action) => button.onClick.AddListener(() => action(_cardData));
}