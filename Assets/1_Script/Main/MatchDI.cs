using System.Linq;
using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;

    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    [SerializeField] DraftTurnSO trait;
    GameBanPickStorage storage;
    PhaseManager phaseManager;
    TraitController traitController;
    PickTableRegistry pickRegistry;

    MatchUI_Controller matchUI_Controller;
    SlotStatusChanger pickStatusChanger;

    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        pickRegistry = new PickTableRegistry(championCatalog);
        new MatchBinder().BindStorageEvents(storage, pickRegistry);
        pickStatusChanger = new SlotStatusChanger(pickRegistry.Statuses);

        matchUI_Controller = GetComponent<MatchUI_Controller>();
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(ban.Turns)),
            new PhaseData(GamePhase.Pick, new Phase(pick.Turns)),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
            new PhaseData(GamePhase.Trait, new Phase(trait.Turns)),
        };
        phaseManager = new(phase);
        utilKey.Init(storage, phaseManager);

        phaseManager.OnPhaseTrait += Trait;
        phaseManager.OnPhaseDone += OnDone;

        matchUI_Controller.Init(storage, phaseManager, pickStatusChanger, pickRegistry); // start보다 먼저
        phaseManager.Start();
    }

    bool initTrait;
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    SlotStorage<ProGamer> gamers = new();
    void Trait(Team team)
    {
        if (initTrait) return;

        gamers.AddSlots(Team.Blue, blueGamers.Select(x => x.CreateGamer()));
        gamers.AddSlots(Team.Red, redGamers.Select(x => x.CreateGamer()));

        ApplyMastery(Team.Blue);
        ApplyMastery(Team.Red);

        traitController = new TraitController(pickRegistry.Statuses);
        traitController.OnTraitUsed += phaseManager.SubmitAction;

        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, pickRegistry);
        initTrait = true;
    }

    // 클래스로 분리 및 테스트
    void ApplyMastery(Team team)
    {
        for (int i = 0; i < gamers.GetTeam(team).Count(); i++)
        {
            var slot = new SlotData(team, i);
            int level = new ActiveMasteryFinder(gamers, pickRegistry.Ids).GetActiveLevel(slot);
            pickStatusChanger.ChangeStat(slot, (stat) => new MasteryApplier().ApplyMastery(stat, level));
        }
    }


    [SerializeField] BonusDataFactory bonusDataSO;
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(pickRegistry.Statuses);
        matchUI_Controller.ShowResult(result);
    }
}
