using UnityEngine;

public class GamePhaseLoder : MonoBehaviour
{
    [SerializeField] DraftTurnSO ban;
    [SerializeField] DraftTurnSO pick;
    [SerializeField] DraftTurnSO trait;

    public PhaseData[] LoadPhase()
    {
        PhaseData[] phase = new PhaseData[]
        {
            new PhaseData(GamePhase.Ban, new Phase(ban.Turns)),
            new PhaseData(GamePhase.Pick, new Phase(pick.Turns)),
            // new PhaseData(GamePhase.Skill, new Phase(trait.Turns)),
        };
        return phase;
    }
}
