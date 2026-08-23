using NUnit.Framework;
using System.Collections.Generic;

public class DeckBuildUIServiceTests
{
    private DeckBuildUIState CreateInitialUIState(int requiredCount, params int[] availableCards)
    {
        var domainState = new DeckBuildState(
            RequiredCount: requiredCount,
            AvailableCards: new HashSet<int>(availableCards),
            SelectedCards: new HashSet<int>()
        );
        return new DeckBuildUIState(domainState);
    }

    [Test]
    public void 카드클릭시_포커스가변경되고_추가버튼이활성화된다()
    {
        // Arrange
        var state = CreateInitialUIState(25, 1, 2);

        // Act: 1번 카드 클릭
        var focusedState = DeckBuildUIService.FocusAvailableCard(state, 1);
        var viewModel = DeckBuildUIService.CreateViewModel(focusedState);

        // Assert
        Assert.AreEqual(1, focusedState.FocusedAvailableCardId);
        Assert.IsTrue(viewModel.AddButton.IsInteractable, "포커스된 카드가 있으므로 추가 버튼이 활성화되어야 합니다.");
    }

    [Test]
    public void 추가버튼클릭시_카드가이동하고_포커스가지워진다()
    {
        // Arrange
        var state = CreateInitialUIState(25, 1, 2);
        var focusedState = DeckBuildUIService.FocusAvailableCard(state, 1);

        // Act: 추가 버튼 클릭 효과
        var nextState = DeckBuildUIService.MoveFocusedToSelected(focusedState);

        // Assert
        Assert.IsTrue(nextState.DomainState.SelectedCards.Contains(1), "1번 카드가 덱으로 이동해야 합니다.");
        Assert.AreEqual(-1, nextState.FocusedAvailableCardId, "이동 후에는 포커스가 초기화되어야 합니다.");
    }

    [Test]
    public void 더블클릭시_포커스와이동이_동시에처리된다()
    {
        // Arrange
        var state = CreateInitialUIState(25, 1, 2);

        // Act: 2번 카드 더블 클릭
        var nextState = DeckBuildUIService.DoubleClickAvailableCard(state, 2);

        // Assert
        Assert.IsTrue(nextState.DomainState.SelectedCards.Contains(2));
        Assert.AreEqual(-1, nextState.FocusedAvailableCardId);
    }

    [Test]
    public void 뷰모델생성_요구치미달시_경고상태가True가된다()
    {
        // Arrange (목표 25장 중 0장)
        var state = CreateInitialUIState(25, 1);

        // Act
        var viewModel = DeckBuildUIService.CreateViewModel(state);

        // Assert
        Assert.AreEqual("0 / 25", viewModel.CountText.Text);
        Assert.IsTrue(viewModel.CountText.IsWarning, "요구치를 채우지 못했으므로 경고(Warning) 상태여야 합니다.");
    }

    [Test]
    public void 덱이꽉차면_사용가능한카드를클릭해도_추가버튼이비활성화된다()
    {
        // Arrange: 요구치 1장짜리 덱에 1장을 더블클릭해서 꽉 채움
        var state = CreateInitialUIState(1, 1, 2);
        var fullState = DeckBuildUIService.DoubleClickAvailableCard(state, 1);

        // Act: 남은 2번 카드 클릭
        var focusedState = DeckBuildUIService.FocusAvailableCard(fullState, 2);
        var viewModel = DeckBuildUIService.CreateViewModel(focusedState);

        // Assert
        Assert.AreEqual(2, focusedState.FocusedAvailableCardId);
        Assert.IsFalse(viewModel.AddButton.IsInteractable, "덱이 꽉 찼으므로 추가 버튼은 비활성화되어야 합니다.");
    }
}