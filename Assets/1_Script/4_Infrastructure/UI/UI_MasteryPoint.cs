using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AAAA : IChampionProvider // 데이터 매니저 만들기?
{
    public ChampionProfile GetProfile(int id)
    {
        var so = ChampionDataLoder.GetChampionData(id);
        return new ChampionProfile(id, so.name, so.StatData, so.Skill);
    }
}

public class UI_MasteryPoint : MonoBehaviour, IMasteryPointView
{
    [Header("프리팹 및 부모")]
    [AssetsOnly][SerializeField] GameObject btnPrefab;
    [SerializeField] Transform parent;

    [Header("포인트 및 챔피언 정보")]
    [SerializeField] TextMeshProUGUI _pointText;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _skillText;
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

    [SerializeField] SkillTextSO skillTextSO;
    MasteryPointPresenter _presenter;

    void Start()
    {
        // Init(new MasteryPointPresenter(new MasteryInventory(ChampionDataLoder.AllId, 10), new ChampionTextBuilder(new AAAA(), skillTextSO.CreateSkillTextBuilder(), new ChampionStatusTextBuilder()), this));
    }

    public void Init(MasteryPointPresenter presenter)
    {
        _presenter = presenter;

        BindUpgradeButtons();
        CreateAndBindChampionButtons();

        _presenter.Initialize();
        ClearDetailView();
    }

    void BindUpgradeButtons()
    {
        _upAttBtn.onClick.AddListener(() => _presenter.RequestUpgrade(StatType.Attack));
        _upDefBtn.onClick.AddListener(() => _presenter.RequestUpgrade(StatType.Defense));
        _upSpdBtn.onClick.AddListener(() => _presenter.RequestUpgrade(StatType.Speed));
    }

    void CreateAndBindChampionButtons()
    {
        foreach (Transform child in parent) Destroy(child.gameObject);

        var buttons = new ChampionButtonCreator().DrawChampionButtons(parent, ChampionDataLoder.AllChampions, btnPrefab);

        foreach (var btn in buttons)
        {
            int id = btn.GetComponent<ChampionIdentify>().Id;
            btn.onClick.AddListener(() => _presenter.SelectChampion(id));
        }
    }

    public void UpdatePoints(int points)
    {
        _pointText.text = $"보유 Point : {points}";
    }

    public void UpdateChampionDetail(ChampionTextModel champModel, MasteryLevelModel masteryModel)
    {
        _nameText.text = champModel.Name;
        _skillText.text = champModel.SkillText;

        _baseAttText.text = champModel.StatModel.Attack;
        _baseDefText.text = champModel.StatModel.Defense;
        _baseSpdText.text = champModel.StatModel.Speed;

        _masteryAttText.text = masteryModel.AttackText;
        _masteryDefText.text = masteryModel.DefenseText;
        _masterySpdText.text = masteryModel.SpeedText;
    }

    void ClearDetailView()
    {
        _nameText.text = "챔피언을 선택하세요";
        _skillText.text = "";
        _baseAttText.text = "공 : -";
        _baseDefText.text = "방 : -";
        _baseSpdText.text = "속 : -";
        _masteryAttText.text = "공 : -";
        _masteryDefText.text = "방 : -";
        _masterySpdText.text = "속 : -";
    }
}