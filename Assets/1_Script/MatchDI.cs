using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManager champManager;
    [SerializeField] BanPickUI BanPickUI;
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

        BanPickUI.SetTeam(Team.Blue);
        //MatchManager matchManager = new MatchManager(phaseManager, draftController, BanPickUI, BanPickUI);

        //matchManager.GameStart();
    }
}
