using NUnit.Framework;
using System.Collections.Generic;

public class PriorityBanPickTests
{
    HashSet<int> CreateIds(params int[] allIds) => new HashSet<int>(allIds);
    PrioritySelector CreateSelector(params int[] selectPlan) => new PrioritySelector(selectPlan);
    [Test]
    public void 우선순위대로_선택()
    {
        var ai = CreateSelector(selectPlan: new[] { 2, 1, 20 });

        Assert.AreEqual(2, ai.Select(CreateIds(2, 1, 4, 5, 6)));
        Assert.AreEqual(1, ai.Select(CreateIds(1, 4, 5, 6)));
    }

    [Test]
    public void 픽_모든후보가_불가하면_랜덤으로_선택한다()
    {
        var ai = CreateSelector(selectPlan: new[] { 2, 3 });

        int selected = ai.Select(CreateIds(7));

        Assert.AreEqual(7, selected);
    }

    [Test]
    public void 우선순위에_있는_모든_id가_밴_불가하면_픽_예정_id를_제외하고_랜덤_선택한다()
    {
        var ids = CreateIds(2);
        var ai = CreateSelector(selectPlan: new[] { 5, 6 });

        int select = ai.Select(ids);

        Assert.AreEqual(2, select);
    }

    [Test]
    public void 픽_숙련도_총합이_가장_높은_빌드_선택()
    {
        var selectable = new HashSet<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 });
        var masteryManager = new MasteryStatCollection(new ChampionMastery[] { new ChampionMastery(1, 10), new ChampionMastery(2, 20), new ChampionMastery(3, 30)});
        var selector1 = CreateSelector(4, 5);
        var selector2 = CreateSelector(1, 2, 4);
        var selector3 = CreateSelector(3, 2, 1);

        var sut = new MultiPrioritySelector(masteryManager, new PrioritySelector[] { selector1, selector2, selector3 } );

        Assert.AreEqual(3, sut.Select(selectable));
    }
}
