using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MasteryPoint : MonoBehaviour
{
    [Header("프리팹 및 부모")]
    [AssetsOnly][SerializeField] UI_CardItem cardPrefab;
    [SerializeField] Transform parent;

    [Header("포인트 및 챔피언 정보")]
    [SerializeField] Button _resetButton;
    [SerializeField] TextMeshProUGUI _pointText;
    [SerializeField] ChampionView _campionView;
    [SerializeField] TextMeshProUGUI _baseAttText;
    [SerializeField] TextMeshProUGUI _baseDefText;
    [SerializeField] TextMeshProUGUI _baseSpdText;

    [SerializeField] Button _upAttBtn;
    [SerializeField] Button _upDefBtn;
    [SerializeField] Button _upSpdBtn;

    [SerializeField] Button _downAttBtn;
    [SerializeField] Button _downDefBtn;
    [SerializeField] Button _downSpdBtn;

    MasteryPointPresenter _presenter;
    IEnumerable<UI_CardItem> _cards;

    public void Init(MasteryPointPresenter presenter)
    {
        _presenter = presenter;

        BindUpgradeButtons();
        CreateAndBindChampionButtons();
        _resetButton.onClick.AddListener(ResetView);

        ClearDetailView();
        UpdatePoint(presenter.CurrentPoint);
    }

    void BindUpgradeButtons()
    {
        _upAttBtn.onClick.AddListener(() => ClickUpgradeBtn(StatType.Attack));
        _upDefBtn.onClick.AddListener(() => ClickUpgradeBtn(StatType.Defense));
        _upSpdBtn.onClick.AddListener(() => ClickUpgradeBtn(StatType.Speed));

        _downAttBtn.onClick.AddListener(() => ClickDowngradeBtn(StatType.Attack));
        _downDefBtn.onClick.AddListener(() => ClickDowngradeBtn(StatType.Defense));
        _downSpdBtn.onClick.AddListener(() => ClickDowngradeBtn(StatType.Speed));
    }

    void ClickUpgradeBtn(StatType statType)
    {
        _presenter.UpgradeCardStat(statType);
        RefreshUI();
    }

    void ClickDowngradeBtn(StatType statType)
    {
        _presenter.DowngradeCardStat(statType);
        RefreshUI();
    }

    void CreateAndBindChampionButtons()
    {
        _cards = ChampionButtonCreator.CreateCardItems(parent, ChampionDataLoder.AllChampions, cardPrefab);

        foreach (var card in _cards)
            card.AddClickListener(champ => SelectChampion(champ.Id));
        RefreshBtnColor();
    }

    void SelectChampion(int id)
    {
        _presenter.SelectChampion(id);
        RefreshUI();
    }

    public void UpdatePoint(int points)
    {
        _pointText.text = $"보유 Point : {points}";
        RefreshBtnColor();
    }

    void RefreshBtnColor()
    {
        if (_cards == null) return;
        foreach (var card in _cards)
            card.SetButtonColor(_presenter.GetButtonColor(card.Id));
    }

    void RefreshUI()
    {
        if (_presenter.HasSelectedChampion)
            RefreshUI(_presenter.CreateModel());
    }

    public void RefreshUI(CardUpgradeViewModel model)
    {
        UpdatePoint(model.UpgradePoint);

        var championModel = model.ChampionModel;
        _campionView.UpdateChampionAll(championModel);

        _baseAttText.text = championModel.StatModel.Attack;
        _baseDefText.text = championModel.StatModel.Defense;
        _baseSpdText.text = championModel.StatModel.Speed;
    }

    void ClearDetailView()
    {
        _baseAttText.text = "공 : -";
        _baseDefText.text = "방 : -";
        _baseSpdText.text = "속 : -";
    }

    void ResetView()
    {
        _presenter.ResetMastery();
        if (_presenter.HasSelectedChampion) RefreshUI();
        else UpdatePoint(_presenter.CurrentPoint);
    }
}