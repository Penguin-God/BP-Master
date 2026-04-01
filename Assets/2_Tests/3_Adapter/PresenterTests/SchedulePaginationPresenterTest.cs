using NUnit.Framework;
using System.Collections.Generic;

public class SchedulePaginationPresenterTest
{
    SchedulePaginationPresenter CreateSut(int matchCount, int currentIndex, int playerId)
    {
        var matches = new List<MatchData>();
        for (int i = 0; i < matchCount; i++)
        {
            // 5번 인덱스에만 플레이어 아이디를 넣고, 나머지는 1, 2로 대충 채웁니다.
            matches.Add(new MatchData(i == 5 ? playerId : 1, 2)); 
        }

        var flow = new ScheduleFlow(matches, currentIndex);
        return new SchedulePaginationPresenter(flow, playerId);
    }

    [Test]
    public void 생성시_현재_진행중인_매치가_포함된_페이지로_자동_초기화된다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 12, playerId: 99);

        Assert.AreEqual(1, presenter.CurrentPage); // 12번 인덱스는 1페이지 (10~19)
    }

    [Test]
    public void 페이지의_데이터를_가져오면_상태가_올바르게_매핑된다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 2, playerId: 99);

        var pageData = presenter.GetCurrentPageData();

        Assert.AreEqual(10, pageData.Count);

        Assert.AreEqual(MatchState.Past, pageData[0].State);     // 0번: 이미 지나간 매치 (0 < 2)
        Assert.AreEqual(MatchState.Current, pageData[2].State);  // 2번: 현재 진행할 매치 (2 == 2)
        Assert.AreEqual(MatchState.Player, pageData[5].State);   // 5번: 플레이어 포함 미래 매치
        Assert.AreEqual(MatchState.Normal, pageData[8].State);   // 8번: 일반 AI 미래 매치
    }

    [Test]
    public void 다음_페이지로_이동하면_다음_10개의_데이터를_반환한다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 5, playerId: 99);

        presenter.NextPage();
        var pageData = presenter.GetCurrentPageData();

        Assert.AreEqual(10, pageData[0].MatchIndex);
    }

    [Test]
    public void 마지막_페이지에서는_남은_개수만큼만_반환하며_더_이상_넘어가지_않는다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 34, playerId: 99);

        presenter.NextPage();

        var pageData = presenter.GetCurrentPageData();
        Assert.AreEqual(3, presenter.CurrentPage);
        Assert.AreEqual(5, pageData.Count);
    }

    [Test]
    public void 첫_페이지에서는_이전으로_넘어가지_않는다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 2, playerId: 99);

        presenter.PrevPage();

        Assert.AreEqual(0, presenter.CurrentPage);
    }
}