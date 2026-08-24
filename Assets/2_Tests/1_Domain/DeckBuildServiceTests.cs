using NUnit.Framework;
using System.Collections.Generic;

public class DeckBuildServiceTests
{
    // 테스트용 초기 상태 생성 도우미 함수
    private DeckBuildState CreateInitialState(int requiredCount, params int[] availableCards)
    {
        return new DeckBuildState(
            RequiredCount: requiredCount,
            AvailableCards: new HashSet<int>(availableCards),
            SelectedCards: new HashSet<int>()
        );
    }

    [Test]
    public void 카드추가_시_덱으로_이동한다()
    {
        // Arrange
        var state = CreateInitialState(25, 1, 2, 3);

        // Act
        var nextState = DeckBuildService.AddCard(state, 1);

        // Assert
        Assert.IsFalse(nextState.AvailableCards.Contains(1), "사용 가능한 카드에서 사라져야 합니다.");
        Assert.IsTrue(nextState.SelectedCards.Contains(1), "내 덱에 카드가 추가되어야 합니다.");

        // 원본 상태 보존 테스트 (불변성 검증)
        Assert.IsTrue(state.AvailableCards.Contains(1), "함수형이므로 원본 상태는 훼손되지 않아야 합니다.");
    }

    [Test]
    public void 카드추가_시_요구치를채운상태면_추가되지않는다()
    {
        // Arrange: 1장만 고를 수 있는 덱에 1, 2번 카드가 사용 가능
        var state = CreateInitialState(1, 1, 2);

        // Act
        var state1 = DeckBuildService.AddCard(state, 1); // 1장 추가 성공 (요구치 도달)
        var state2 = DeckBuildService.AddCard(state1, 2); // 2번째 장 추가 시도

        // Assert
        Assert.AreEqual(1, state2.SelectedCards.Count, "요구치를 초과하여 추가될 수 없습니다.");
        Assert.IsFalse(state2.SelectedCards.Contains(2), "두 번째 카드는 덱에 들어가지 않아야 합니다.");
    }

    [Test]
    public void 카드제거_시_사용가능한_카드로_이동한다()
    {
        // Arrange
        var state = CreateInitialState(25, 1, 2);
        var addedState = DeckBuildService.AddCard(state, 1);

        // Act
        var removedState = DeckBuildService.RemoveCard(addedState, 1);

        // Assert
        Assert.IsTrue(removedState.AvailableCards.Contains(1), "덱에서 뺀 카드는 다시 사용 가능한 상태가 되어야 합니다.");
        Assert.IsFalse(removedState.SelectedCards.Contains(1), "내 덱에서 카드가 사라져야 합니다.");
    }

    [Test]
    public void 요구치를채웠을때만_덱완성_반환()
    {
        // Arrange
        var state = CreateInitialState(requiredCount: 1, 1, 2, 3);

        // Act & Assert
        Assert.IsFalse(DeckBuildService.IsDeckComplete(state), "요구치가 충족되지 않으면 false여야 합니다.");

        var state1 = DeckBuildService.AddCard(state, 2);
        Assert.IsTrue(DeckBuildService.IsDeckComplete(state1), "요구치가 충족되면 true여야 합니다.");
    }
}