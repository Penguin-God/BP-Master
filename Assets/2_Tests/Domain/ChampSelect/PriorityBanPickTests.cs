using NUnit.Framework;
using System;
using System.Collections.Generic;

public class PriorityBanPickTests
{
    HashSet<int> CreateIds(params int[] allIds) => new HashSet<int>(allIds);
    PrioritySelector CreateSut(int[] banPlan, int[] pickPlan) => new PrioritySelector(banPlan, pickPlan);

    [Test]
    public void 우선순위대로_저장하며_앞에_게_없으면_다음_순번_선택()
    {
        var ai = CreateSut(banPlan: new[] { 20, 10 }, pickPlan: new[] { 2, 1 });

        Assert.AreEqual(2, ai.Pick(CreateIds(2, 1, 4, 5, 6)));
        Assert.AreEqual(1, ai.Pick(CreateIds(1, 4, 5, 6)));

        Assert.AreEqual(20, ai.Ban(CreateIds(20, 10, 4, 5, 6)));
        Assert.AreEqual(10, ai.Ban(CreateIds(10, 4, 5, 6)));
    }

    [Test]
    public void 픽_모든후보가_불가하면_랜덤으로_선택한다()
    {
        var ai = CreateSut(banPlan: Array.Empty<int>(), pickPlan: new[] { 2, 3 });

        int selected = ai.Pick(CreateIds(7));

        Assert.AreEqual(7, selected);
    }

    [Test]
    public void 밴_모든후보가_불가하면_픽_예정_id를_제외하고_랜덤_선택한다()
    {
        var ids = CreateIds(2, 3, 4);
        var ai = CreateSut(banPlan: new[] { 10, 20 }, pickPlan: new[] { 3, 4 });

        int banned = ai.Ban(ids);

        Assert.AreEqual(2, banned);
    }
}
