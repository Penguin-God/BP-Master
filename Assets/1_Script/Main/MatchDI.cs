using System.Collections.Generic;
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
    PickTableRegistry pickFacade;

    MatchUI_Controller matchUI_Controller;

    SlotStorage<ProGamer> gamers = new();
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        pickFacade = new PickTableRegistry(championCatalog);
        new MatchBinder().BindStorageEvents(storage, pickFacade);
        
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

        matchUI_Controller.Init(storage, phaseManager); // start보다 먼저
        phaseManager.Start();
    }

    bool initTrait;
    [SerializeField] ProGamerSO[] blueGamers;
    [SerializeField] ProGamerSO[] redGamers;
    void Trait(Team team)
    {
        if (initTrait) return;

        gamers.AddSlots(Team.Blue, blueGamers.Select(x => x.CreateGamer()));
        gamers.AddSlots(Team.Red, redGamers.Select(x => x.CreateGamer()));

        ApplyMastery(Team.Blue);
        ApplyMastery(Team.Red);

        traitController = new TraitController(pickFacade.Statuses);
        traitController.OnTraitUsed += phaseManager.SubmitAction;

        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, pickFacade);
        initTrait = true;
    }

    // 클래스로 분리 및 테스트
    void ApplyMastery(Team team)
    {
        for (int i = 0; i < pickFacade.Statuses.GetTeam(team).Count(); i++)
        {
            var slot = new SlotData(team, i);
            var status = pickFacade.Statuses.GetSlot(slot);
            var beforeStat = status.StatData;
            int id = pickFacade.Champions.GetSlot(slot).Id;
            print(slot.Team);
            print(slot.Index);
            int level = gamers.GetSlot(slot).GetMastery(id);

            new MasteryApplier().ApplyMastery(status, level);
            matchUI_Controller.UpdateMaserty(new StatChangeData(slot, beforeStat, status.StatData));
        }
    }

    // 클래스로 분리 및 테스트
    void OnDone()
    {
        var builder = new MatchResultBuilder(bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultConverter(builder).ToResult(pickFacade.Statuses);
        matchUI_Controller.ShowResult(result);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
}
