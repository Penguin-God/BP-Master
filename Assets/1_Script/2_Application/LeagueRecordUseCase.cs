
public class LeagueRecordUseCase
{
    readonly ILeagueRecordStorage storage;

    public LeagueRecordUseCase(ILeagueRecordStorage storage) => this.storage = storage;

    public LeagueRecordCollection RecordMatch(int player1Id, int player1Wins, int player2Id, int player2Wins)
    {
        var collection = storage.LoadAll();

        var p1Record = collection.Get(player1Id).ApplyMatchResult(player1Wins, player2Wins);
        var p2Record = collection.Get(player2Id).ApplyMatchResult(player2Wins, player1Wins);

        collection.Update(p1Record);
        collection.Update(p2Record);

        storage.SaveAll(collection);

        return collection;
    }
}