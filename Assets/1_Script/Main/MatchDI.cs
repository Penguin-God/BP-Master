using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;
    GameBanPickStorage storage;

    PhaseManager phaseManager;
    MatchUI_Controller matchUI_Controller;
    ChampionStorageFactory storageFactory;

    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);

        matchUI_Controller = GetComponent<MatchUI_Controller>();
        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase());
        utilKey.Init(storage, phaseManager);

        phaseManager.OnPhaseTrait += Trait;
        phaseManager.OnPhaseDone += OnDone;
        storageFactory = new ChampionStorageFactory(championCatalog);

        matchUI_Controller.Init(storage, phaseManager, storageFactory); // start보다 먼저
        phaseManager.Start();
    }

    bool initTrait;
    SlotStorage<ChampionStatus> statuses = new();
    void Trait(Team team)
    {
        if (initTrait) return;

        statuses = storageFactory.CreateStatusStorage(storage.PickIds);

        var traitController = new TraitController(statuses);
        traitController.OnTraitUsed += phaseManager.SubmitAction;

        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, storageFactory.CreateChampionStorage(storage.PickIds), statuses);

        ApplyMastery(); // 마지막에
        initTrait = true;
    }

    void ApplyMastery() => new TeamMasteryApplier().Apply(GetComponent<PlayerRoster>().Rosters, storage.PickIds, statuses);


    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(statuses);
        matchUI_Controller.ShowResult(result);
    }
}
