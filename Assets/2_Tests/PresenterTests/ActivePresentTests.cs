using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class ActivePresentTests
{
    [Test]
    public void 타겟은_정해진_수_초과랑_중복_불가()
    {
        var presenter = new ActivePresenter();
        presenter.SelectTrait(new Trait(Side.All, null), 2);

        Assert.IsTrue(presenter.AddTarget(6));
        Assert.IsFalse(presenter.AddTarget(6)); // 중복 불가
        Assert.IsTrue(presenter.AddTarget(4));
        Assert.IsFalse(presenter.AddTarget(1)); // 타겟 끝
    }
}
