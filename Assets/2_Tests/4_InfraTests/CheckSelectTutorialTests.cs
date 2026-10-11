using NUnit.Framework;

public class CheckSelectTutorialTests
{
    [Test]
    public void 선택이_정답이면_성공_콜백_실행()
    {
        // Arrange
        int[] answers = { 1, 2, 3 };
        bool isSuccessCalled = false;
        bool isFailedCalled = false;

        var sut = new CheckSelectTutorial(
            answers,
            onSuccess: () => isSuccessCalled = true,
            onFailed: () => isFailedCalled = true
        );

        // Act (정답 ID 전달)
        sut.Select(2);

        // Assert
        Assert.IsTrue(isSuccessCalled);
        Assert.IsFalse(isFailedCalled);
    }

    [Test]
    public void 선택이_오답이면_실패_콜백_실행()
    {
        // Arrange
        int[] answers = { 1, 2, 3 };
        bool isSuccessCalled = false;
        bool isFailedCalled = false;

        var sut = new CheckSelectTutorial(
            answers,
            onSuccess: () => isSuccessCalled = true,
            onFailed: () => isFailedCalled = true
        );

        // Act (정답에 없는 ID 전달)
        sut.Select(999);

        // Assert
        Assert.IsFalse(isSuccessCalled);
        Assert.IsTrue(isFailedCalled);
    }
}