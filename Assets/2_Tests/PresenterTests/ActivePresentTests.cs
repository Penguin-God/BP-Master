using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class ActivePresentTests
{
    //[Test]
    //public void SelectTrait_자동타겟_적용성공()
    //{
    //    var appliedTargets = new List<int>();
    //    var trait = new Trait(
    //        maxTarget: 2,
    //        passiveAction: targets => appliedTargets.AddRange(targets),
    //        autoTargetProvider: () => new[] { 10, 11, 12 } // 2개만 필요하므로 10,11 사용
    //    );

    //    var presenter = new ActivePresenter();
    //    presenter.SelectTrait(trait, targetCount: 2); // == MaxTarget → 자동 타겟

    //    var result = presenter.ApplyTrait();

    //    Assert.IsTrue(result);
    //    CollectionAssert.AreEquivalent(new[] { 10, 11 }, appliedTargets);
    //}


    [Test]
    public void 타겟은_정해진_수_초과랑_중복_불가()
    {
        var presenter = new ActivePresenter(5);
        presenter.SelectTrait(new Trait(Side.All, null), targetCount: 2);

        Assert.IsTrue(presenter.AddTarget(6));
        Assert.IsFalse(presenter.AddTarget(6)); // 중복 불가
        Assert.IsTrue(presenter.AddTarget(4));
        Assert.IsFalse(presenter.AddTarget(1)); // 타겟 끝
    }

    //[Test]
    //public void SelectTrait_수동타겟_제한갯수_검증과_적용성공()
    //{
    //    var appliedTargets = new List<int>();
    //    var trait = new Trait(
    //        maxTarget: 3,
    //        passiveAction: targets => appliedTargets.AddRange(targets)
    //    );

    //    var presenter = new ActivePresenter();
    //    presenter.SelectTrait(trait, targetCount: 2); // < MaxTarget → 자동 설정 없음

    //    // 아직 부족하므로 적용 실패
    //    Assert.IsFalse(presenter.ApplyTrait());

    //    // AddTarget은 요청 개수(2)를 초과할 수 없음
    //    Assert.IsTrue(presenter.AddTarget(5));
    //    Assert.IsTrue(presenter.AddTarget(6));
    //    Assert.IsFalse(presenter.AddTarget(7)); // 초과 시 false
    //    Assert.IsFalse(presenter.AddTarget(6)); // 중복 방지

    //    // 정확히 2개이므로 이제 적용 성공
    //    var result = presenter.ApplyTrait();
    //    Assert.IsTrue(result);
    //    CollectionAssert.AreEquivalent(new[] { 5, 6 }, appliedTargets);
    //}
}
