
public class GameFlowTextBuilder
{
    public string BuildFlowText(GameFlowData flow) => $"{BuildTurnText(flow.Turn)} {BuildPhaseText(flow.Phase)} 단계";

    string BuildPhaseText(GamePhase phase)
    {
        switch (phase)
        {
            case GamePhase.Ban: return "밴";
            case GamePhase.Pick: return "픽";
            case GamePhase.Skill: return "특성";
            case GamePhase.Done: return "끝";
            default: return "";
        }
    }

    string BuildTurnText(Team team)
    {
        switch (team)
        {
            case Team.Blue: return "파랑 팀";
            case Team.Red: return "빨강 팀";
            case Team.All: return "양팀";
            default: return "";
        }
    }
}