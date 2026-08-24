using System.Collections.Generic;

public record DeckBuildState(int RequiredCount, HashSet<int> AvailableCards, HashSet<int> SelectedCards);

public enum CardPoolType
{
    Available,
    Selected
}

public record CardIdentity(CardPoolType Pool, int Id);