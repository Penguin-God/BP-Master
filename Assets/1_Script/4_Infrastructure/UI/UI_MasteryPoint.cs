using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MasteryPoint : MonoBehaviour
{
    [Header("프리팹 및 부모")]
    [AssetsOnly][SerializeField] GameObject btnPrefab;
    [SerializeField] Transform parent;

    [Header("포인트 및 챔피언 정보")]
    [SerializeField] Button _resetButton;
    [SerializeField] TextMeshProUGUI _pointText;
    [SerializeField] ChampionView _campionView;
    [SerializeField] TextMeshProUGUI _baseAttText;
    [SerializeField] TextMeshProUGUI _baseDefText;
    [SerializeField] TextMeshProUGUI _baseSpdText;

    [Header("마스터리 정보 및 버튼")]
    [SerializeField] TextMeshProUGUI _masteryAttText;
    [SerializeField] TextMeshProUGUI _masteryDefText;
    [SerializeField] TextMeshProUGUI _masterySpdText;
    [SerializeField] Button _upAttBtn;
    [SerializeField] Button _upDefBtn;
    [SerializeField] Button _upSpdBtn;

    MasteryPointPresenter _presenter;
    MasteryProfile _masteryProfile;
    IEnumerable<Button> buttons;

    public void Init(MasteryPointPresenter presenter, MasteryProfile masteryProfile)
    {
        _presenter = presenter;
        _masteryProfile = masteryProfile;

        BindUpgradeButtons();
        CreateAndBindChampionButtons();
        _resetButton.onClick.AddListener(ResetView);

        ClearDetailView();
        UpdatePoint();
    }

    void BindUpgradeButtons()
    {
        _upAttBtn.onClick.AddListener(() => ClickUpgradeBtn(StatType.Attack));
        _upDefBtn.onClick.AddListener(() => ClickUpgradeBtn(StatType.Defense));
        _upSpdBtn.onClick.AddListener(() => ClickUpgradeBtn(StatType.Speed));
    }

    void ClickUpgradeBtn(StatType statType)
    {
        if (!_presenter.HasSelectedChampion) return;

        _presenter.UpgradeCardStat(statType);
        RefreshUI();
    }

    void CreateAndBindChampionButtons()
    {
        foreach (Transform child in parent) Destroy(child.gameObject);

        buttons = ChampionButtonCreator.CreateChampionButtons(parent, ChampionDataLoder.AllChampions, btnPrefab);

        foreach (var btn in buttons)
        {
            int id = btn.GetComponent<ChampionIdentify>().Id;
            btn.onClick.AddListener(() => SelectChampion(id));
        }
        RefreshBtnColor();
    }

    void SelectChampion(int id)
    {
        _presenter.SelectChampion(id);
        RefreshUI();
    }

    public void UpdatePoints(int points)
    {
        _pointText.text = $"보유 Point : {points}";
        RefreshBtnColor();
    }

    void RefreshBtnColor()
    {
        foreach (var btn in buttons)
        {
            int id = btn.GetComponent<ChampionIdentify>().Id;
            if (_masteryProfile.TryGetBoard(id, out _)) ButtonUtil.ChangeButtonColor(btn, Color.green);
            else ButtonUtil.ChangeButtonColor(btn, Color.white);
        }
    }

    void RefreshUI() => RefreshUI(_presenter.CreateModel());

    public void RefreshUI(CardUpgradeViewModel model)
    {
        UpdatePoints(model.UpgradePoint);

        var championModel = model.ChampionModel;
        _campionView.UpdateChampionAll(championModel);

        _baseAttText.text = championModel.StatModel.Attack;
        _baseDefText.text = championModel.StatModel.Defense;
        _baseSpdText.text = championModel.StatModel.Speed;

        _masteryAttText.text = model.MasteryModel.AttackText;
        _masteryDefText.text = model.MasteryModel.DefenseText;
        _masterySpdText.text = model.MasteryModel.SpeedText;
    }

    void ClearDetailView()
    {
        _baseAttText.text = "공 : -";
        _baseDefText.text = "방 : -";
        _baseSpdText.text = "속 : -";
        _masteryAttText.text = "공 : -";
        _masteryDefText.text = "방 : -";
        _masterySpdText.text = "속 : -";
    }

    void ResetView()
    {
        _presenter.ResetMastery();
        if (_presenter.HasSelectedChampion) RefreshUI();
        else UpdatePoint();
    }

    void UpdatePoint() => UpdatePoints(_masteryProfile.AvailablePoints);
}