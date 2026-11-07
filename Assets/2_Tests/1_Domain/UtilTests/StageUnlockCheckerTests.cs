using NUnit.Framework;

public class StageUnlockCheckerTests
{
    [TestCase(0, 0, true)]
    [TestCase(1, 0, false)]
    [TestCase(0, 2, true)]
    [TestCase(2, 2, true)]
    [TestCase(3, 2, false)]
    public void 스테이지_인덱스와_클리어_스테이지에_따라_해금_여부를_올바르게_판정한다(int stageIndex, int clearedStage, bool expected)
    {
        var result = StageUnlockChecker.IsUnlocked(stageIndex, clearedStage);

        Assert.AreEqual(expected, result);
    }

    [TestCase(4, 0, new[] { true, false, false, false })]
    [TestCase(4, 2, new[] { true, true, true, false })]
    [TestCase(4, 3, new[] { true, true, true, true })]
    public void 전체_스테이지_개수와_클리어_스테이지에_따라_해금_목록을_올바르게_반환한다(int stageCount, int clearedStage, bool[] expected)
    {
        var sut = StageUnlockChecker.GetUnlockStates(stageCount, clearedStage);

        CollectionAssert.AreEqual(expected, sut);
    }
}
