using NUnit.Framework;
using System.Collections.Generic;

public class SchedulePaginationPresenterTest
{
    SchedulePaginationPresenter CreateSut(int matchCount, int currentIndex, int playerId)
    {
        var matches = new List<MatchData>();
        for (int i = 0; i < matchCount; i++)
        {
            // 인덱스 15번 매치에만 플레이어(ID: 99)를 넣습니다.
            int id1 = i == 15 ? playerId : i * 2;
            int id2 = i * 2 + 1;
            matches.Add(new MatchData(id1, id2));
        }

        var flow = new ScheduleFlow(matches, currentIndex);
        return new SchedulePaginationPresenter(flow, playerId);
    }

    [Test]
    public void 생성시_현재_진행중인_매치가_포함된_페이지로_자동_초기화된다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 12, playerId: 99); // 13번째 매치 (인덱스 12)

        Assert.AreEqual(1, presenter.CurrentPage); // 0이 첫 페이지이므로 1은 11~20번 페이지
    }

    [Test]
    public void 페이지의_데이터를_가져오면_상태가_올바르게_매핑된다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 12, playerId: 99);

        var pageData = presenter.GetCurrentPageData();

        Assert.AreEqual(10, pageData.Count);

        // 인덱스 10번 (11번째 매치) -> 지난 매치
        Assert.AreEqual(MatchState.Past, pageData[0].State);
        Assert.AreEqual(10, pageData[0].MatchIndex);

        // 인덱스 12번 (13번째 매치) -> 현재 매치
        Assert.AreEqual(MatchState.Current, pageData[2].State);

        // 인덱스 15번 (16번째 매치) -> 플레이어 매치
        Assert.AreEqual(MatchState.Player, pageData[5].State);

        // 인덱스 18번 (19번째 매치) -> 일반 매치
        Assert.AreEqual(MatchState.Normal, pageData[8].State);
    }

    [Test]
    public void 다음_페이지로_이동하면_다음_10개의_데이터를_반환한다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 5, playerId: 99); // 1페이지 (0~9)

        presenter.NextPage(); // 2페이지 (10~19)
        var pageData = presenter.GetCurrentPageData();

        Assert.AreEqual(10, pageData[0].MatchIndex); // 페이지의 첫 요소 인덱스 확인
    }

    [Test]
    public void 마지막_페이지에서는_남은_개수만큼만_반환하며_더_이상_넘어가지_않는다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 34, playerId: 99); // 마지막 4페이지 (30~34)

        presenter.NextPage(); // 범위를 넘어서려는 시도

        var pageData = presenter.GetCurrentPageData();
        Assert.AreEqual(3, presenter.CurrentPage); // 페이지가 증가하지 않음
        Assert.AreEqual(5, pageData.Count); // 30, 31, 32, 33, 34 -> 5개 반환
    }

    [Test]
    public void 첫_페이지에서는_이전으로_넘어가지_않는다()
    {
        var presenter = CreateSut(matchCount: 35, currentIndex: 2, playerId: 99); // 첫 페이지

        presenter.PrevPage();

        Assert.AreEqual(0, presenter.CurrentPage);
    }
}