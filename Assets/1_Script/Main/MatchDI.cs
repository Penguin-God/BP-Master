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
    PickFacade selectFacade;

    MatchUI_Controller matchUI_Controller;

    Dictionary<Team, IReadOnlyList<ProGamer>> gamerMap = new();
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        selectFacade = new PickFacade(championCatalog);
        storage.OnPick += selectFacade.Pick;

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

        gamerMap.Add(Team.Blue, blueGamers.Select(x => x.CreateGamer()).ToList());
        gamerMap.Add(Team.Red, redGamers.Select(x => x.CreateGamer()).ToList());

        ApplyMastery(Team.Blue);
        ApplyMastery(Team.Red);

        traitController = new TraitController(selectFacade.Statuses);
        traitController.OnTraitUsed += phaseManager.SubmitAction;

        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, selectFacade.Champions);
        initTrait = true;
    }

    // 클래스로 분리 및 테스트
    void ApplyMastery(Team team)
    {
        for (int i = 0; i < selectFacade.Champions.GetTeam(team).Count(); i++)
        {
            var slot = new SlotData(team, i);
            var beforeStat = selectFacade.Champions.GetSlot(slot).StatData;

            if (new MasteryApplier().ApplyMastery(gamerMap[team][i], selectFacade.Champions.GetSlot(slot)))
                matchUI_Controller.UpdateMaserty(new StatChangeData(slot, beforeStat, selectFacade.Champions.GetSlot(slot).StatData));
        }
    }

    // 클래스로 분리 및 테스트
    void OnDone()
    {
        var blue = selectFacade.Champions.GetTeam(Team.Blue);
        var red = selectFacade.Champions.GetTeam(Team.Red);

        MatchResult result = new MatchResultCalculator(bonusDataSO.TeamBonus).CalculateResult(blue.Select(x => x.StatData), red.Select(x => x.StatData));
        matchUI_Controller.ShowResult(result);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
}
