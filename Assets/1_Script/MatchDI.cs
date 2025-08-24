using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManager champManager;
    [SerializeField] BanPickUI BanPickUI;
    MatchManager matchManager;
    GameBanPickStorage storage;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(champManager.AllId);
        DraftActionController draftController = new(storage);

        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red, Team.Red, Team.Blue, Team.Blue, Team.Red})),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        PhaseActionDispatcher blue = new PhaseActionDispatcher(Team.Blue, BanPickUI);
        PhaseActionDispatcher red = new PhaseActionDispatcher(Team.Red, BanPickUI); ;
        matchManager = new MatchManager(phaseManager, draftController, blue, red);

        matchManager.GameStart();
        BanPickUI.Init();
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
