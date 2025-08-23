using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManager champManager;
    [SerializeField] BanPickUI BanPickUI;
    MatchManager matchManager;
    public void GameStart(Team playerTeam)
    {
        var storage = new GameBanPickStorage(champManager.AllId);
        DraftActionController draftController = new(storage);

        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
        };
        PhaseManager phaseManager = new(phase);

        PhaseActionDispatcher blue = new PhaseActionDispatcher(Team.Blue, BanPickUI);
        PhaseActionDispatcher red = new PhaseActionDispatcher(Team.Red, BanPickUI); ;
        matchManager = new MatchManager(phaseManager, draftController, blue, red);

        matchManager.GameStart();
    }

    void Update()
    {
        if (matchManager != null &&  matchManager.CurrentPhase == GamePhase.Done) print("끝!!!!!!!!!!!!");
    }
}
