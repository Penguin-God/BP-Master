using UnityEngine;

public class MatchDI : MonoBehaviour
{
    [SerializeField] ChampionManager champManager;
    [SerializeField] BanPickUI BanPickUI;
    GameBanPickStorage storage;
    PhaseManager phaseManager;
    public void GameStart(Team playerTeam)
    {
        storage = new GameBanPickStorage(champManager.AllId);
        
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(new Team[] { Team.Blue, Team.Red })),
            new PhaseData(GamePhase.Pick, new Phase(new Team[] { Team.Blue, Team.Red, Team.Red, Team.Blue, Team.Blue, Team.Red})),
            new PhaseData(GamePhase.Swap, new Phase(new Team[] { Team.All })),
            new PhaseData(GamePhase.Active, new Phase(new Team[] { Team.Blue, Team.Red })),
        };
        phaseManager = new(phase);
        phaseManager.OnFlowChanged += new PhaseActionDispatcher(BanPickUI, BanPickUI).OnRequestAction;

        BanPickUI.Init(storage, phaseManager); // start보다 먼저. 
        phaseManager.Start();
    }

    [SerializeField] BonusDataFactory bonusDataSO;
    bool isActiveSet = false;
    void Update()
    {
        if (phaseManager != null && phaseManager.CurrentFlow.Phase == GamePhase.Active && isActiveSet == false)
        {
            StatManager statManager = new StatManager(champManager.GetStats(storage.GetStorage(Team.Blue, SelectType.Pick)), champManager.GetStats(storage.GetStorage(Team.Red, SelectType.Pick)));
            ActiveExcuter blueAct = new ActiveExcuter(statManager, Team.Blue, new Trait[] { new Trait(Side.Opponent, new AttackChanger(-10)) });
            ActiveExcuter redAct = new ActiveExcuter(statManager, Team.Red, new Trait[] { new Trait(Side.Opponent, new AttackChanger(-10)) });
            ActiveExcuteManager activeExcuteManager = new ActiveExcuteManager(blueAct, redAct);
            BanPickUI.SetActiveExcutor(activeExcuteManager);
            isActiveSet = true;
        }

        if (phaseManager != null && phaseManager.CurrentFlow.Phase == GamePhase.Done)
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
