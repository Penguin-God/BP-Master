using System.Collections.Generic;
using NUnit.Framework;
using static TestHelper;

public class SlotUIStateTests
{
    class SpySlotView : ISlotView
    {
        public int StopAnimationCallCount { get; private set; }
        public SlotUIState CurrentState { get; private set; }
        public ChampionStatData LastUpdatedStat { get; private set; }

        public void StopCurrentAnimation() => StopAnimationCallCount++;
        public void SetState(SlotUIState state) => CurrentState = state;
        public void UpdateStatView(ChampionStatData stat) => LastUpdatedStat = stat;
    }

    ChampionStatus CreateStatusSut(int attack, int defense) => CreateStatus(attack, defense);

    SlotPresenter CreateSut(SlotData slot, ChampionStatus status, int championId, CardDetailView detailView, ISlotView view) => new SlotPresenter(slot, status, championId, detailView, view);

    [TestCase(35, 20, 20)]
    [TestCase(50, 15, 30)]
    [TestCase(10, 1, 1)]
    public void 카드가_사망_판정을_받으면_방어력뿐만_아니라_공격력도_즉시_0으로_초기화된다(int initialAttack, int initialDefense, int damage)
    {
        var sut = CreateStatusSut(initialAttack, initialDefense);
        ChampionStatData notifiedAfter = new ChampionStatData(initialAttack, initialDefense);
        sut.OnStatChanged += (_, after) => notifiedAfter = after;

        sut.TakeDamage(damage);

        Assert.IsTrue(sut.IsDead);
        Assert.AreEqual(0, sut.Stat.Attack);
        Assert.AreEqual(0, sut.Stat.Defense);
        Assert.AreEqual(new ChampionStatData(0, 0), notifiedAfter);
    }

    [TestCase(GamePhase.Ban, false)]
    [TestCase(GamePhase.Pick, false)]
    [TestCase(GamePhase.Skill, false)]
    [TestCase(GamePhase.Done, false)]
    public void 밴_픽_상대_AI_턴_페이즈에서_생존_카드는_Default_사망_카드는_Dead로_상태가_결정된다(GamePhase phase, bool isPlayerCasting)
    {
        var storage = new SlotStorage<ChampionStatus>();
        storage.AddSlot(Team.Blue, CreateStatusSut(20, 30));
        storage.AddSlot(Team.Blue, CreateStatusSut(25, 0));
        storage.AddSlot(Team.Red, CreateStatusSut(15, 10));
        storage.AddSlot(Team.Red, CreateStatusSut(40, 0));

        var sut = SlotStateResolver.ResolveBoardByGamePhase(phase, isPlayerCasting, false, storage);

        Assert.AreEqual(SlotUIState.Default, sut[BlueZeroSlot]);
        Assert.AreEqual(SlotUIState.Dead, sut[BlueOneSlot]);
        Assert.AreEqual(SlotUIState.Default, sut[RedZeroSlot]);
        Assert.AreEqual(SlotUIState.Dead, sut[RedOneSlot]);
    }

    [TestCase(true, SlotUIState.AttackCasting)]
    [TestCase(false, SlotUIState.SkillCasting)]
    public void 플레이어_공격_및_스킬_시전_페이즈_진입_시_행동_주체와_유효_대상_비타깃_생존_사망_카드가_올바르게_분기된다(bool isAttack, SlotUIState expectedCasterState)
    {
        var storage = new SlotStorage<ChampionStatus>();
        storage.AddSlot(Team.Blue, CreateStatusSut(20, 30));
        storage.AddSlot(Team.Blue, CreateStatusSut(15, 25));
        storage.AddSlot(Team.Red, CreateStatusSut(10, 20));
        storage.AddSlot(Team.Red, CreateStatusSut(30, 0));
        var validTargets = new HashSet<SlotData> { RedZeroSlot };

        var sut = SlotStateResolver.ResolveBoardByGamePhase(GamePhase.Pick, true, isAttack, storage, BlueZeroSlot, validTargets);

        Assert.AreEqual(expectedCasterState, sut[BlueZeroSlot]);
        Assert.AreEqual(SlotUIState.Untargetable, sut[BlueOneSlot]);
        Assert.AreEqual(SlotUIState.Targetable, sut[RedZeroSlot]);
        Assert.AreEqual(SlotUIState.Untargetable, sut[RedOneSlot]);
    }

    [TestCase(SlotUIState.Targetable, SlotUIState.AttackHit)]
    [TestCase(SlotUIState.AttackCasting, SlotUIState.Default)]
    [TestCase(SlotUIState.SkillCasting, SlotUIState.SkillImpact)]
    [TestCase(SlotUIState.Untargetable, SlotUIState.Dead)]
    public void 프레젠터에_새_상태가_주입되면_기존_상태가_즉시_대체되어_항상_단일_상태만_유지된다(SlotUIState firstState, SlotUIState secondState)
    {
        var view = new SpySlotView();
        var sut = CreateSut(BlueZeroSlot, CreateStatusSut(20, 20), 101, new CardDetailView(), view);

        sut.SetState(firstState);
        sut.SetState(secondState);

        Assert.AreEqual(secondState, sut.CurrentState);
        Assert.AreEqual(secondState, view.CurrentState);
        Assert.GreaterOrEqual(view.StopAnimationCallCount, 2);
    }

    [TestCase(SlotUIState.AttackHit, 20, SlotUIState.Default)]
    [TestCase(SlotUIState.AttackHit, 0, SlotUIState.Dead)]
    [TestCase(SlotUIState.SkillImpact, 15, SlotUIState.Default)]
    [TestCase(SlotUIState.SkillImpact, 0, SlotUIState.Dead)]
    public void 공격_및_스킬_적용_연출_완료_후_생존_카드는_Default로_사망_카드는_Dead로_복귀한다(SlotUIState impactState, int remainingDefense, SlotUIState expectedRestoredState)
    {
        var status = CreateStatusSut(25, 20);
        var view = new SpySlotView();
        var sut = CreateSut(RedZeroSlot, status, 102, new CardDetailView(), view);
        sut.SetState(impactState);
        if (remainingDefense <= 0) status.TakeDamage(20);

        sut.OnImpactCompleted();

        Assert.AreEqual(expectedRestoredState, sut.CurrentState);
        Assert.AreEqual(expectedRestoredState, view.CurrentState);
    }

    [TestCase(SlotUIState.Default, 1)]
    [TestCase(SlotUIState.Dead, 1)]
    [TestCase(SlotUIState.Untargetable, 0)]
    [TestCase(SlotUIState.AttackCasting, 0)]
    [TestCase(SlotUIState.SkillCasting, 0)]
    [TestCase(SlotUIState.AttackHit, 0)]
    [TestCase(SlotUIState.SkillImpact, 0)]
    public void Default_또는_Dead_상태에서_슬롯을_클릭하면_CardDetailView_업데이트가_호출되고_Untargetable_상태에서_클릭하면_무시된다(SlotUIState state, int expectedUpdateCount)
    {
        var detailView = new CardDetailView();
        var view = new SpySlotView();
        var sut = CreateSut(BlueZeroSlot, CreateStatusSut(30, 40), 7, detailView, view);
        sut.SetState(state);

        sut.OnClick();

        Assert.AreEqual(expectedUpdateCount, detailView.UpdateCallCount);
        if (expectedUpdateCount > 0)
            Assert.AreEqual(7, detailView.LastUpdatedChampionId);
    }
}
