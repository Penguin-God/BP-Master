using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;
    GameBanPickStorage storage;
    PickTableRegistry pickRegistry;
    PickSlotRegistry pickSlotRegistry;

    PhaseManager phaseManager;
    MatchUI_Controller matchUI_Controller;
    SlotStatusChanger pickStatusChanger;

    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        pickRegistry = new PickTableRegistry(championCatalog);
        pickSlotRegistry = new PickSlotRegistry(GetComponent<PlayerRoster>().Rosters);
        storage.OnPick += pickRegistry.Pick;
        storage.OnPick += pickSlotRegistry.Pick;
        pickStatusChanger = new SlotStatusChanger(pickRegistry.Statuses);

        matchUI_Controller = GetComponent<MatchUI_Controller>();
        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase());
        utilKey.Init(storage, phaseManager);

        phaseManager.OnPhaseTrait += Trait;
        phaseManager.OnPhaseDone += OnDone;

        matchUI_Controller.Init(storage, phaseManager, pickStatusChanger, pickRegistry); // start보다 먼저
        phaseManager.Start();
    }

    bool initTrait;
    void Trait(Team team)
    {
        if (initTrait) return;

        ApplyMastery();

        var traitController = new TraitController(pickRegistry.Statuses);
        traitController.OnTraitUsed += phaseManager.SubmitAction;

        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, pickRegistry);
        initTrait = true;
    }

    void ApplyMastery() => new TeamMasteryApplier(pickStatusChanger).Apply(pickSlotRegistry.PickSlotDatas);


    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(pickRegistry.Statuses);
        matchUI_Controller.ShowResult(result);
    }
}
