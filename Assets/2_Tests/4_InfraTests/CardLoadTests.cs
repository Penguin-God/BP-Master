using NUnit.Framework;
using System.Linq;

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

    [Test]
    public void 스테이지의_카드들을_합쳐서_반환해야함()
    {
        CardListSO[] result = CardListLoder.LoadStage(1);
        int count1 = result[0].CardIdSet.Count;
        int count2 = result[1].CardIdSet.Count;

        Assert.AreEqual(count1 + count2, CardListLoder.LoadAllStageCards(1).Count());
    }
}
