using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManagerMono champManager;
    [SerializeField] ChampionSelectUI_Controller BanPickUI;
    [SerializeField] TraitUseView traitUseView;
    [SerializeField] BanPickView banPickView;
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    [SerializeField] DraftTurnSO trait;
    GameBanPickStorage storage;
    PhaseManager phaseManager;
    IReadOnlyDictionary<Team, IReadOnlyList<Champion>> pickChampions;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(champManager.AllId);
        
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
    void Trait(Team team)
    {
        if (initTrait) return;
        // 그냥 밴픽 끝나면 알아서 넣어주면 안되냐
        pickChampions = storage.TeamPicks.ToDictionary(x => x.Key, x => (IReadOnlyList<Champion>)x.Value.Select(x => champManager.GetChampion(x)).ToList());
        var presenter = new TraitUsePresenter(new TraitController(pickChampions));
        traitUseView.Init(presenter);
        phaseManager.OnPhaseTrait += traitUseView.UpdateTrait;
        traitUseView.UpdateTrait(team);
        presenter.OnTraitUsed += phaseManager.SubmitAction;
        presenter.OnTraitUsed += _ => banPickView.UpdateAllPick(pickChampions);
        initTrait = true;
    }

    void OnDone() // pickChampions 사용
    {
        var blue = champManager.GetStats(storage.GetStorage(Team.Blue, SelectType.Pick));
        var red = champManager.GetStats(storage.GetStorage(Team.Red, SelectType.Pick));

        var calculator = new TeamScoreCalculator(bonusDataSO.ChampionBonus, bonusDataSO.TeamBonus);
        MatchResult result = new MatchResultCalculator(calculator).CalculateResult(blue, red);
        print(result.BlueScore);
        print(result.RedScore);
        print(result.Winner);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
}
