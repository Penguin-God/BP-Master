using UnityEngine;

[CreateAssetMenu(fileName = "GamePhaseLoderSO", menuName = "BP Master/GamePhaseLoder")]
public class GamePhaseLoderSO : ScriptableObject
{
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO ban2;
    [SerializeField] DraftTurnSO pick;
    [SerializeField] DraftTurnSO pick2;

    public PhaseData[] LoadPhase()
    {
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(ban.Turns)),
            new PhaseData(GamePhase.Pick, new Phase(pick.Turns)),
            new PhaseData(GamePhase.Ban, new Phase(ban2.Turns)),
            new PhaseData(GamePhase.Pick, new Phase(pick2.Turns)),
        };
        return phase;
    }

    public PhaseAdvancer CreateAdvacer() => new PhaseAdvancer(LoadPhase());
}
