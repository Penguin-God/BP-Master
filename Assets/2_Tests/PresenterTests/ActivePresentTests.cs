using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class ActivePresentTests
{
    [Test]
    public void 챔_선택_안하고_특성_사용시_False()
    {
        TraitPresenter traitPresenter = new TraitPresenter();

        Assert.IsFalse(traitPresenter.UseTrait(1));
    }
}
