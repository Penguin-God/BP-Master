using NUnit.Framework;
using System;
using System.Collections.Generic;

public class PriorityBanPickTests
{
    HashSet<int> CreateIds(params int[] allIds) => new HashSet<int>(allIds);
    PrioritySelector CreateSelector(int[] banPlan, params int[] pickPlan) => new PrioritySelector(banPlan, pickPlan);
    [Test]
    public void 우선순위대로_선택()
    {
        var ai = CreateSelector(banPlan: new[] { 20, 10 }, pickPlan: new[] { 2, 1, 20 });

        Assert.AreEqual(2, ai.Pick(CreateIds(2, 1, 4, 5, 6)));
        Assert.AreEqual(1, ai.Pick(CreateIds(1, 4, 5, 6)));

        Assert.AreEqual(20, ai.Ban(CreateIds(20, 10, 4, 5, 6)));
        Assert.AreEqual(10, ai.Ban(CreateIds(10, 4, 5, 6)));
    }

    [Test]
    public void 픽_모든후보가_불가하면_랜덤으로_선택한다()
    {
        var ai = CreateSelector(banPlan: Array.Empty<int>(), pickPlan: new[] { 2, 3 });

        int selected = ai.Pick(CreateIds(7));

        Assert.AreEqual(7, selected);
    }

    [Test]
    public void 우선순위에_있는_모든_id가_밴_불가하면_픽_예정_id를_제외하고_랜덤_선택한다()
    {
        var ids = CreateIds(2, 3, 4);
        var ai = CreateSelector(banPlan: new[] { 10, 20 }, pickPlan: new[] { 3, 4 });

        int banned = ai.Ban(ids);

        Assert.AreEqual(2, banned);
    }



    [Test]
    public void 픽_숙련도_총합이_가장_높은_빌드_선택()
    {
        var selectable = new HashSet<int>(new int[] { 1, 2, 3, 4, 5, 6, 7 });
        var masteryManager = new MasteryCollection(new ChampionMastery[] { new ChampionMastery(1, 10), new ChampionMastery(2, 20), new ChampionMastery(3, 30)});
        var selector1 = CreateSelector(new[] { 5 }, 4, 5);
        var selector2 = CreateSelector(new[] { 6 }, 1, 2, 4);
        var selector3 = CreateSelector(new[] { 7 }, 3, 2, 1);

        var sut = new MultiPrioritySelector(masteryManager, new PrioritySelector[] { selector1, selector2, selector3 } );

        Assert.AreEqual(7, sut.Ban(selectable));
        Assert.AreEqual(3, sut.Pick(selectable));
    }
}
