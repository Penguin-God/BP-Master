public record DeckBuildUIState(DeckBuildState DomainState, int FocusedAvailableCardId = -1, int FocusedSelectedCardId = -1);

// --- 화면에 그리기 위한 뷰 모델(View Model)들 ---
public record CountTextViewModel(string Text, bool IsWarning);
public record ButtonViewModel(bool IsInteractable);

// UI가 화면을 업데이트할 때 넘겨받을 최종 데이터 묶음
public record DeckBuildViewModel(
    CountTextViewModel CountText,
    ButtonViewModel AddButton,
    ButtonViewModel RemoveButton
);

public static class DeckBuildUIService
{
    public static DeckBuildUIState FocusAvailableCard(DeckBuildUIState state, int cardId)
    {
        if (state.DomainState.AvailableCards.Contains(cardId) == false) return state;
        return state with { FocusedAvailableCardId = cardId };
    }

    // 2. 내 덱 카드 단일 클릭 (포커스)
    public static DeckBuildUIState FocusSelectedCard(DeckBuildUIState state, int cardId)
    {
        if (!state.DomainState.SelectedCards.Contains(cardId)) return state;
        return state with { FocusedSelectedCardId = cardId };
    }

    // 3. 포커스된 카드를 내 덱으로 이동 (추가 버튼 클릭)
    public static DeckBuildUIState MoveFocusedToSelected(DeckBuildUIState state)
    {
        if (state.FocusedAvailableCardId == -1) return state;

        var nextDomain = DeckBuildService.AddCard(state.DomainState, state.FocusedAvailableCardId);

        // 이동 성공 시 포커스를 해제하고 상태 업데이트
        if (nextDomain != state.DomainState)
        {
            return state with
            {
                DomainState = nextDomain,
                FocusedAvailableCardId = -1
            };
        }
        return state;
    }

    // 4. 포커스된 카드를 사용 가능한 카드로 이동 (제거 버튼 클릭)
    public static DeckBuildUIState MoveFocusedToAvailable(DeckBuildUIState state)
    {
        if (state.FocusedSelectedCardId == -1) return state;

        var nextDomain = DeckBuildService.RemoveCard(state.DomainState, state.FocusedSelectedCardId);

        if (nextDomain != state.DomainState)
        {
            return state with
            {
                DomainState = nextDomain,
                FocusedSelectedCardId = -1
            };
        }
        return state;
    }

    // 5. 더블 클릭 로직 (포커스 + 이동을 동시에 처리)
    public static DeckBuildUIState DoubleClickAvailableCard(DeckBuildUIState state, int cardId)
    {
        var focusedState = FocusAvailableCard(state, cardId);
        return MoveFocusedToSelected(focusedState);
    }

    public static DeckBuildUIState DoubleClickSelectedCard(DeckBuildUIState state, int cardId)
    {
        var focusedState = FocusSelectedCard(state, cardId);
        return MoveFocusedToAvailable(focusedState);
    }

    // 6. UI에 바인딩할 뷰 모델 생성
    public static DeckBuildViewModel CreateViewModel(DeckBuildUIState state)
    {
        var domain = state.DomainState;
        bool isWarning = domain.SelectedCards.Count < domain.RequiredCount;
        bool isFull = domain.SelectedCards.Count >= domain.RequiredCount;

        return new DeckBuildViewModel(
            CountText: new CountTextViewModel($"{domain.SelectedCards.Count} / {domain.RequiredCount}", isWarning),
            AddButton: new ButtonViewModel(state.FocusedAvailableCardId != -1 && !isFull),
            RemoveButton: new ButtonViewModel(state.FocusedSelectedCardId != -1)
        );
    }
}