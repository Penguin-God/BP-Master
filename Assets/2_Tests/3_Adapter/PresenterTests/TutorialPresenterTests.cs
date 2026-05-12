using NUnit.Framework;

public class TutorialPresenterTests
{
    TutorialPresenter CreateSut() => new TutorialPresenter(new string[] { "첫 번째 대사", "두 번째 대사" });

    [Test]
    public void 튜토리얼을_시작하고_Advance를_호출하면_첫_대사를_반환한다()
    {
        var presenter = CreateSut();

        bool hasNext = presenter.Advance(out string text);

        Assert.IsTrue(hasNext);
        Assert.AreEqual("첫 번째 대사", text);
    }

    [Test]
    public void 모든_대사가_끝나면_진행을_못하는_false반환()
    {
        var presenter = CreateSut();

        presenter.Advance(out _); // 첫 번째 대사 통과
        presenter.Advance(out _); // 두 번째 대사 통과

        bool hasNext = presenter.Advance(out string text);

        Assert.IsFalse(hasNext);
        Assert.IsEmpty(text);
    }
}