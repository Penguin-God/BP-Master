using System.Collections.Generic;

public record DeckBuildState(int RequiredCount, HashSet<int> AvailableCards, HashSet<int> SelectedCards);