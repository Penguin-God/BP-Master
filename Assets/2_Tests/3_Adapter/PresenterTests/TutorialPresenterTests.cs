using NUnit.Framework;
using System.Collections.Generic;

public class TutorialPresenterTests
{
    TutorialPresenter CreateSut()
    {
        var tutorialData = new Dictionary<int, string[]>
        {
            { 3, new[] { "첫 번째 대사", "두 번째 대사" } }
        };
        return new TutorialPresenter(tutorialData);
    }

    [Test]
    public void 튜토리얼을_시작하고_Advance를_호출하면_첫_대사를_반환한다()
    {
        var presenter = CreateSut();
        presenter.TryStart(3);

        bool hasNext = presenter.Advance(out string text);

        Assert.IsTrue(hasNext);
        Assert.AreEqual("첫 번째 대사", text);
    }

    [Test]
    public void 모든_대사가_끝나면_진행을_못하는_false반환()
    {
        var presenter = CreateSut();
        presenter.TryStart(3);

        presenter.Advance(out _); // 첫 번째 대사 통과
        presenter.Advance(out _); // 두 번째 대사 통과

        bool hasNext = presenter.Advance(out string text);

        Assert.IsFalse(hasNext);
        Assert.IsEmpty(text);
    }

    [Test]
    public void 시작되지_않은_상태에서_Advance를_호출하면_안전하게_false를_반환한다()
    {
        var presenter = CreateSut();

        // TryStart를 호출하지 않음 (또는 없는 인덱스 호출)
        presenter.TryStart(99);

        bool hasNext = presenter.Advance(out string text);

        Assert.IsFalse(hasNext);
        Assert.IsEmpty(text);
    }
}