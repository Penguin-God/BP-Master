using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManager champManager;
    [SerializeField] BanPickUI BanPickUI;
    MatchManager matchManager;
    GameBanPickStorage storage;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(champManager.AllId);
        
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red, Team.Red, Team.Blue, Team.Blue, Team.Red})),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        PhaseActionRequestor blue = new PhaseActionRequestor(Team.Blue, BanPickUI);
        PhaseActionRequestor red = new PhaseActionRequestor(Team.Red, BanPickUI); ;
        var bus = new ActionEventBus();
        matchManager = new MatchManager(phaseManager, bus, blue, red);

        matchManager.GameStart();
        BanPickUI.Init(storage, bus);
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    void Update()
    {
        if (matchManager != null &&  matchManager.CurrentPhase == GamePhase.Done)
        {
            var blue = champManager.GetStats(storage.GetStorage(Team.Blue, SelectType.Pick));
            var red = champManager.GetStats(storage.GetStorage(Team.Red, SelectType.Pick));

            var calculator = new TeamScoreCalculator(bonusDataSO.ChampionBonus, bonusDataSO.TeamBonus);
            MatchResult result = new MatchResultCalculator(calculator).CalculateResult(blue, red);
            print(result.BlueScore);
            print(result.RedScore);
            print(result.Winner);
        }
    }
}
