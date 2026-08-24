using System.Collections.Generic;
using System.Linq;

public record MatchData(int Id1, int Id2)
{
    public IEnumerable<int> All_Ids => new int[] { Id1, Id2 };
    public int GetOpponentId(int id) => All_Ids.Except(new int[] { id }).First();
};


public record DeckBuildState(int CardCount, HashSet<int> AvailableCards, HashSet<int> SelectedCards);
public record CardIdentity(CardPoolType Pool, int Id);

