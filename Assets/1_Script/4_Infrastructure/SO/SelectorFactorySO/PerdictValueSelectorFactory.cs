using UnityEngine;

[CreateAssetMenu(fileName = "PerdictValueSelectSO", menuName = "BP Master/PerdictValueSelectSO")]
public class PredictValueSelectorFactory : AI_SelectorFactory
{
    [SerializeField] ValueSelectSO valueSelectSO;

    public override IChampionSelector CreateBanSelector() => valueSelectSO.CreateBanSelector();

    public override IChampionSelector CreatePickSelector() => new ValueSelector(CreateRanker(team));

    BanPickStorage storage;
    PhaseAdvancer phaseAdvancer;
    public void Inject(BanPickStorage storage, PhaseAdvancer phaseAdvancer)
    {
        valueSelectSO.Init(team, championCatalog, masteryManager, statusSlots);
        this.storage = storage;
        this.phaseAdvancer = phaseAdvancer;
    }

    PredictivePickEvaluator CreateEvaluator(Team team) => new PredictivePickEvaluator(valueSelectSO.CreateEvaluator(team), storage, championCatalog, new SkillPreviewer(), phaseAdvancer, team, statusSlots);

    ChampionRanker CreateRanker(Team team) => new ChampionRanker(championCatalog, CreateEvaluator(team));
}
