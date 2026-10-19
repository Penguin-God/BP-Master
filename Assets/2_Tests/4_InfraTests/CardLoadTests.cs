using NUnit.Framework;

public class CardLoadTests
{
    [Test]
    public void 지정된_스테이지의_번호만큼_로드()
    {
        CardListSO[] result = CardListLoder.LoadStage(2);
        Assert.AreEqual(3, result.Length);
    }

    [Test]
    public void 클리어_스테이지가_없으면_기본만_로드()
    {
        CardListSO[] result = CardListLoder.LoadStage(0);
        Assert.AreEqual(1, result.Length);
    }
}
