using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassiveExcuteTests
{
    [Test]
    public void 액티브는_다_사용해야_넘어감()
    {
        ActiveExcuter sut = new(2);

        sut.Do();
        sut.Do();

        Assert.IsTrue(sut.IsDone);
    }
}
