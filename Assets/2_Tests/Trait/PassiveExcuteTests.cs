using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassiveExcuteTests
{
    [Test]
    public void 액티브는_다_사용하면_끝()
    {
        ActiveExcuter sut = new(new IActivePassive[] { new SelectAttackWeaker(0), new SelectAttackWeaker(0) });

        sut.Do(0);
        Assert.IsFalse(sut.IsDone);

        sut.Do(0);
        Assert.IsTrue(sut.IsDone);
    }
}
