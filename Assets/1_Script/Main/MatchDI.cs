using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;
    GameBanPickStorage storage;
    PickSlotRegistry pickSlotRegistry;

    PhaseManager phaseManager;
    MatchUI_Controller matchUI_Controller;

    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        pickSlotRegistry = new PickSlotRegistry(GetComponent<PlayerRoster>().Rosters);
        storage.OnPick += pickSlotRegistry.Pick;

        matchUI_Controller = GetComponent<MatchUI_Controller>();
        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase());
        utilKey.Init(storage, phaseManager);

        phaseManager.OnPhaseTrait += Trait;
        phaseManager.OnPhaseDone += OnDone;

        matchUI_Controller.Init(storage, phaseManager); // start보다 먼저
        phaseManager.Start();
    }


    SlotStatusChanger pickStatusChanger;
    bool initTrait;
    SlotStorage<ChampionStatus> statuses = new();
    void Trait(Team team)
    {
        if (initTrait) return;

        ChampionStorageFactory storageFactory = new ChampionStorageFactory(championCatalog);
        statuses = storageFactory.CreateStatusStorage(storage.PickIds);

        var traitController = new TraitController(statuses);
        traitController.OnTraitUsed += phaseManager.SubmitAction;

        pickStatusChanger = new SlotStatusChanger(statuses);
        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, storageFactory.CreateChampionStorage(storage.PickIds), pickStatusChanger);

        ApplyMastery(); // 마지막에
        initTrait = true;
    }

    void ApplyMastery() => new TeamMasteryApplier(pickStatusChanger).Apply(GetComponent<PlayerRoster>().Rosters, storage.PickIds);


    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(statuses);
        matchUI_Controller.ShowResult(result);
    }
}
