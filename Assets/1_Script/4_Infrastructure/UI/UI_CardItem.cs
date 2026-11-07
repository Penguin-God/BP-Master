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

    public void DrawCard(ChampionSO championSO)
    {
        _cardData = championSO;
        icon.sprite = _cardData.Icon;
        ChangeName(_cardData.ChampionName);
        ActiveIcon(true);
    }

    public void ChangeName(string name) => nameText.text = name;
    public void ActiveIcon(bool isActive) => icon.gameObject.SetActive(isActive);
    public void DrawModel(CardViewModel model)
    {
        ChangeName(model.Name);
        nameText.color = model.TextColor;
        if (model.IsEnabled)
        {
            ButtonUtil.ActiveButton(button);
            SetButtonColor(model.ButtonColor); // ActiveButton에 색깔 변경 있어서 다음에 둬야 함
        }
        else ButtonUtil.InActiveButton(button);
    }

    public void SetButtonColor(Color color)
    {
        ButtonUtil.ChangeButtonColor(button, color);
    }

    public void InActive()
    {
        ButtonUtil.InActiveButton(button);
        nameText.color = new Color32(60, 60, 60, 255);
    }

    public void AddClickListener(UnityAction<ChampionSO> action) => button.onClick.AddListener(() => action(_cardData));
}