public class LeagueRecordUseCase
{
    readonly ILeagueRecordStorage storage;
    public LeagueRecordUseCase(ILeagueRecordStorage storage) => this.storage = storage;

    public void RecordMatch(int player1Id, int player1Wins, int player2Id, int player2Wins)
    {
        var records = storage.LoadAll();
        records[player1Id] = GetRecord(player1Id).ApplyMatchResult(player1Wins, player2Wins);
        records[player2Id] = GetRecord(player2Id).ApplyMatchResult(player2Wins, player1Wins);
        storage.SaveAll(records);

        LeagueRecord GetRecord(int id) => records.TryGetValue(id, out var result) ? result : new LeagueRecord();
    }
}