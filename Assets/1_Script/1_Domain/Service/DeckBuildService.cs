using System.Collections.Generic;

public static class DeckBuildService
{
    // 사용 가능한 카드를 내 덱에 추가 (사용 가능 -> 선택됨)
    public static DeckBuildState AddCard(DeckBuildState state, int cardId)
    {
        if (!state.AvailableCards.Contains(cardId))
            return state;

        if (state.SelectedCards.Count >= state.CardCount)
            return state;

        // 원본 훼손을 막기 위해 새로운 HashSet 복사본 생성
        var nextAvailable = new HashSet<int>(state.AvailableCards);
        nextAvailable.Remove(cardId);

        var nextSelected = new HashSet<int>(state.SelectedCards);
        nextSelected.Add(cardId);

        return state with
        {
            AvailableCards = nextAvailable,
            SelectedCards = nextSelected
        };
    }

    // 덱에서 선택한 요소를 빼기 (선택됨 -> 사용 가능)
    public static DeckBuildState RemoveCard(DeckBuildState state, int cardId)
    {
        if (!state.SelectedCards.Contains(cardId))
            return state;

        // 원본 훼손을 막기 위해 새로운 HashSet 복사본 생성
        var nextAvailable = new HashSet<int>(state.AvailableCards);
        nextAvailable.Add(cardId);

        var nextSelected = new HashSet<int>(state.SelectedCards);
        nextSelected.Remove(cardId);

        return state with
        {
            AvailableCards = nextAvailable,
            SelectedCards = nextSelected
        };
    }

    public static bool IsDeckComplete(DeckBuildState state) => state.SelectedCards.Count == state.CardCount;
}