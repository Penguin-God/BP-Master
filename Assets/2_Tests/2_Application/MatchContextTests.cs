using NUnit.Framework;
using Match;

public class MatchContextTests
{
    [TearDown]
    public void TearDown()
    {
        MatchContext.Clear();
    }

    [Test]
    public void Init시_매치데이터_저장소_세팅()
    {
        // Arrange
        var allIds = new[] { 10, 20, 30 };

        // Act: 변경된 MatchInit 파라미터 적용 (id1, id2, targetWin, allIds)
        MatchContext.MatchInit(1, 100, targetWin: 2, allIds);

        // Assert: CurrentSeries 레코드 내부의 값을 확인합니다.
        Assert.AreEqual(1, MatchContext.MatchState.Player1.Id);
        Assert.AreEqual(100, MatchContext.MatchState.Player2.Id);
        Assert.AreEqual(2, MatchContext.MatchState.TargetWins);
    }

    [Test]
    public void 피어리스에_포함된_id는_저장소에서_제외된다()
    {
        // Arrange
        var allIds = new[] { 10, 20, 30 };
        MatchContext.MatchInit(1, 100, 2, allIds);

        // Act
        MatchContext.RecordMatchResult(new int[] { 10 });
        var result = MatchContext.CreateFearlessStorage().SelectableIds;

        // Assert
        CollectionAssert.DoesNotContain(result, 10);
        CollectionAssert.Contains(result, 20); // 명확성을 위해 정상 카드 유지 여부 추가 검증
    }

    [Test]
    public void 승수를_채워서_매치가_끝나면_모든_상태를_비운다()
    {
        // Arrange
        MatchContext.MatchInit(1, 100, 2, new[] { 10 });

        // Act & Assert
        Assert.IsFalse(MatchContext.EndMatch(1), "1승이므로 아직 게임이 끝나지 않아야 합니다.");
        Assert.IsTrue(MatchContext.EndMatch(1), "2승(TargetWin)을 달성했으므로 게임이 끝나고 상태가 초기화되어야 합니다.");

        // 매치가 완전히 종료된 후 Clear가 제대로 작동했는지 확인
        Assert.IsNull(MatchContext.MatchState, "Clear 호출 시 CurrentSeries는 null이 되어야 합니다.");
        Assert.IsEmpty(MatchContext.FearlessLockedCards, "Clear 호출 시 피어리스 잠금 목록도 비워져야 합니다.");
    }
}