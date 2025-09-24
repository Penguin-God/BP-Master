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
    PickFacade pickFacade;

    MatchUI_Controller matchUI_Controller;

    SlotStorage<ProGamer> gamers;
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        pickFacade = new PickFacade(championCatalog);
        new MatchBinder().BindStorageEvents(storage, pickFacade);
        storage.OnPick += pickFacade.Pick;

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
        for (int i = 0; i < pickFacade.Champions.GetTeam(team).Count(); i++)
        {
            var slot = new SlotData(team, i);
            var beforeStat = pickFacade.Champions.GetSlot(slot).StatData;

            new MasteryApplier().ApplyMastery(pickFacade.Statuses.GetSlot(slot), gamers.GetSlot(slot).GetMastery(pickFacade.Champions.GetSlot(slot).Id));
            matchUI_Controller.UpdateMaserty(new StatChangeData(slot, beforeStat, pickFacade.Champions.GetSlot(slot).StatData));

            // gamers.GetLevel(slot)
        }
    }

    // 클래스로 분리 및 테스트
    void OnDone() => matchUI_Controller.ShowResult(new MatchResultConverter(new MatchResultBuilder(bonusDataSO.TeamBonus)).ToResult(pickFacade.Statuses));
    //{
    //    var blue = pickFacade.Statuses.GetTeam(Team.Blue);
    //    var red = pickFacade.Statuses.GetTeam(Team.Red);

    //    MatchResult result = new MatchResultBuilder(bonusDataSO.TeamBonus).CalculateResult(blue.Select(x => x.StatData), red.Select(x => x.StatData));
    //    matchUI_Controller.ShowResult(result);
    //}

    [SerializeField] BonusDataFactory bonusDataSO;
}
