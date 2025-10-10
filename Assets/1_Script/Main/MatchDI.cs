using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;
    GameBanPickStorage storage;

    PhaseManager phaseManager;
    PhaseEventDispatcher phaseEventDispatcher = new PhaseEventDispatcher();
    MatchUI_Controller matchUI_Controller;
    IdStorageConverter storageFactory;
    GamerRoster gamerRoster;

    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);

        matchUI_Controller = GetComponent<MatchUI_Controller>();

        phaseManager = new(GetComponent<GamePhaseLoder>().LoadPhase(), phaseEventDispatcher);
        utilKey.Init(storage, phaseManager);

        phaseEventDispatcher.OnPhaseTrait += Trait;
        phaseEventDispatcher.OnPhaseDone += OnDone;
        storageFactory = new IdStorageConverter(championCatalog);

        gamerRoster  = GetComponent<GamerRoster>();
        gamerRoster.SetRandomRoster(championCatalog);

        matchUI_Controller.Init(storage, phaseManager, storageFactory, phaseEventDispatcher); // start보다 먼저

        var ai = new AI_SelectAgent(Team.Red, phaseManager, storage, new RandomSelector());
        phaseEventDispatcher.OnPhaseBan += ai.Ban;
        phaseEventDispatcher.OnPhasePick += ai.Pick;

        phaseManager.Start();
    }

    bool initTrait;
    SlotStorage<ChampionStatus> statuses = new();
    void Trait(Team team)
    {
        if (initTrait) return;

        statuses = storageFactory.IdToStatus(storage.PickIds);

        var traitFacade = new TraitUseFacade(statuses);
        traitFacade.OnTraitUsed += phaseManager.SubmitAction;

        var trait_ai = new AI_TraitAgent(Team.Red, new TraitSlotFilter(5, traitFacade), new ChampionStorageConverter().ChamptionToTrait(storageFactory.IdToChampion(storage.PickIds)), traitFacade);
        phaseEventDispatcher.OnPhaseTrait += trait_ai.UseTrait;

        matchUI_Controller.TraitUI_Init(team, phaseEventDispatcher, traitFacade, storageFactory.IdToChampion(storage.PickIds), statuses);

        ApplyMastery(); // 마지막에
        initTrait = true;
    }

    void ApplyMastery() => new TeamMasteryApplier().Apply(gamerRoster.Rosters, storage.PickIds, statuses);


    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(statuses);
        matchUI_Controller.ShowResult(result);
    }
}
