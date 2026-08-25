using System.Collections.Generic;
using System.Linq;

public record MatchData(int Id1, int Id2);

public record DeckBuildState(int CardCount, HashSet<int> AvailableCards, HashSet<int> SelectedCards)
{
    public HashSet<int> ChangeableCards => AvailableCards.Except(SelectedCards).ToHashSet();
};
public record CardIdentity(CardPoolType Pool, int Id);

public record MatchParticipant(int Id, int Wins = 0);
public record MatchSeriesState(MatchParticipant Player1, MatchParticipant Player2, int TargetWins)
{
    public int GetOpponentId(int id) => id == Player1.Id ? Player2.Id : Player1.Id;
    public int GetWin(int id) => id == Player1.Id ? Player1.Wins : (id == Player2.Id ? Player2.Wins : 0);
    public int TotalWins => Player1.Wins + Player2.Wins;
    public bool IsMatchFinished => Player1.Wins >= TargetWins || Player2.Wins >= TargetWins;

    public MatchSeriesState AddWin(int winnerId)
    {
        if (winnerId == Player1.Id) return this with { Player1 = Player1 with { Wins = Player1.Wins + 1 } };
        else if (winnerId == Player2.Id) return this with { Player2 = Player2 with { Wins = Player2.Wins + 1 } };
        return this; // 무승부거나 잘못된 ID일 경우 원본 그대로 반환
    }
}