using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManagerMono champManager;
    ChampionCatalog championCatalog;
    [SerializeField] ChampionSelectUI_Controller BanPickUI;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    [SerializeField] DraftTurnSO trait;
    GameBanPickStorage storage;
    PhaseManager phaseManager;

    IReadOnlyDictionary<Team, IReadOnlyList<Champion>> pickChampions;
    SlotStorage pickSlotStorage;
    Dictionary<Team, IReadOnlyList<ProGamer>> gamerMap = new();
    public void GameStart(Team playerTeam)
    {
        championCatalog = new ChampionCatalog(Resources.LoadAll<ChampionSO>("SO/Champions").Select(x => x.CreateChampion()));
        storage = new GameBanPickStorage(championCatalog.AllId);
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(ban.Turns)),
            new PhaseData(GamePhase.Pick, new Phase(pick.Turns)),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
            new PhaseData(GamePhase.Trait, new Phase(trait.Turns)),
        };
        phaseManager = new(phase);

        phaseManager.OnPhaseSwap += BanPickUI.OnSwap;
        phaseManager.OnPhaseTrait += Trait;
        phaseManager.OnPhaseDone += OnDone;

        BanPickUI.Init(storage, phaseManager); // start보다 먼저. 
        phaseManager.Start();

        traitUseView.gameObject.SetActive(false);
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

        // 지우고
        var presenter = new  TraitUsePresenter(new TraitController(pickChampions));
        traitUseView.Init(presenter);
        phaseManager.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.UpdateTrait(team);
        presenter.OnTraitUsed += phaseManager.SubmitAction;
        // 지우기
        presenter.OnTraitUsed += _ => banPickView.UpdateAllPick(pickChampions);
        initTrait = true;
    }

    void SettingSlotStorage()
    {
        pickSlotStorage = new SlotStorage();
        foreach (var item in storage.PickBySlot)
            pickSlotStorage.AddSlot(item.Key.Team, championCatalog.GetChampion(item.Value));
    }

    void ApplyMastery(Team team)
    {
        for (int i = 0; i < pickSlotStorage.GetTeam(team).Count(); i++)
            new MasteryApplier().ApplyMastery(gamerMap[team][i], pickChampions[team][i]);
    }

    void OnDone()
    {
        var blue = pickSlotStorage.GetTeam(Team.Blue);
        var red = pickSlotStorage.GetTeam(Team.Red);

        var calculator = new TeamScoreCalculator(bonusDataSO.ChampionBonus, bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultCalculator(calculator).CalculateResult(blue.Select(x => x.StatData), red.Select(x => x.StatData));
        print($"Blue : {result.BlueScore}");
        print($"Red : {result.RedScore}");
        print($"승자 : {result.Winner}");
    }

    [SerializeField] BonusDataFactory bonusDataSO;
}
