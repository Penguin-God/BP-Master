using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionHandleTests
{
    [Test]
    public void 요청하면_그에_맞는_행동_실행()
    {
        IActionHandler sut = new TestActionHandler();
        GameBanPickStorage result = new GameBanPickStorage(new int[] { 1 });
        DraftActionController draftAction = new DraftActionController(result);

        draftAction.ChangePhase(GamePhase.Pick, Team.Red);
        sut.OnRequestAction(draftAction, GamePhase.Pick);

        Assert.AreEqual(1, result.GetStorage(Team.Red, SelectType.Pick)[0]);
    }
}

public class TestActionHandler : IActionHandler
{
    public void OnRequestAction(DraftActionController draftAction, GamePhase phase)
    {
        draftAction.Pick(Team.Red, 1);
    }
}
