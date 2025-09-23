using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Playables;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionRepository champManager;
    ChampionCatalog championCatalog => champManager.Catalog;
    [SerializeField] ChampionSelectUI_Controller BanPickUI;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    [SerializeField] DraftTurnSO trait;
    GameBanPickStorage storage;
    PhaseManager phaseManager;
    TraitController traitController;
    SelectFacade selectFacade;

    [SerializeField] ScoreView scoreView;
    MatchUI_Controller matchUI_Controller;
    SlotStorage<Champion> pickSlotStorage;
    Dictionary<Team, IReadOnlyList<ProGamer>> gamerMap = new();
    [SerializeField] UtilKey utilKey;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(championCatalog.AllId);
        selectFacade = new SelectFacade(championCatalog);
        storage.OnPick += selectFacade.Pick;

        matchUI_Controller = GetComponent<MatchUI_Controller>();
        utilKey.Init(storage);
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(ban.Turns)),
            new PhaseData(GamePhase.Pick, new Phase(pick.Turns)),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
            new PhaseData(GamePhase.Trait, new Phase(trait.Turns)),
        };
        phaseManager = new(phase);

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

        // 그냥 밴픽 끝나면 알아서 넣어주면 안되나
        SettingSlotStorage();
        ApplyMastery(Team.Blue);
        ApplyMastery(Team.Red);
        // 쳄피언 스트레지 넣어줘
        traitController = new TraitController(selectFacade.Statuses);
        matchUI_Controller.TraitUI_Init(team, phaseManager, traitController, selectFacade.Champions);
        initTrait = true;
    }

    void SettingSlotStorage()
    {
        pickSlotStorage = new SlotStorage<Champion>();
        foreach (var item in storage.PickBySlot)
            pickSlotStorage.AddSlot(item.Key.Team, championCatalog.GetChampion(item.Value));
    }

    void ApplyMastery(Team team)
    {
        for (int i = 0; i < pickSlotStorage.GetTeam(team).Count(); i++)
        {
            var slot = new SlotData(team, i);
            var beforeStat = pickSlotStorage.GetSlot(slot).StatData;
            // 숙련도 있으면 UI 피드백
            if (new MasteryApplier().ApplyMastery(gamerMap[team][i], pickSlotStorage.GetSlot(slot)))
                banPickView.ChangeChampionStat(new StatChangeData(slot, beforeStat, pickSlotStorage.GetSlot(slot).StatData));
        }
    }

    [SerializeField] GameObject Scores;
    [SerializeField] TextMeshProUGUI textBlue;
    [SerializeField] TextMeshProUGUI textRed;
    void OnDone()
    {
        var blue = pickSlotStorage.GetTeam(Team.Blue);
        var red = pickSlotStorage.GetTeam(Team.Red);

        MatchResult result = new MatchResultCalculator(bonusDataSO.TeamBonus).CalculateResult(blue.Select(x => x.StatData), red.Select(x => x.StatData));
        matchUI_Controller.ShowResult(result);
    }


    [SerializeField] BonusDataFactory bonusDataSO;
}
