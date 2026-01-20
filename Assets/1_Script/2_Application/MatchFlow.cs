public class MatchFlowUsecase
{
    private readonly MatchRecord _record;
    private readonly Team _playerTeam;

    public MatchFlowUsecase(MatchRecord record, Team playerTeam)
    {
        _record = record;
        _playerTeam = playerTeam;
    }

    public void ProcessResult(Team winnerTeam)
    {
        if (winnerTeam == Team.All) return;

        Participant winner = (winnerTeam == _playerTeam) ? Participant.Player : Participant.AI;
        _record.AddWin(winner);
    }
}