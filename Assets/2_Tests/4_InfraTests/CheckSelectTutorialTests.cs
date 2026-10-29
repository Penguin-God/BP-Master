using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class CheckSelectTutorialTests
{
    CheckSelectTutorial CreateSut(IEnumerable<int> answers, Action onSuccess, Action onFailed) => new CheckSelectTutorial(answers, onSuccess, onFailed);
    CheckSelectTutorial CreateSut(TutorialQuizConditionData conditionData, Action onSuccess, Action onFailed) => new CheckSelectTutorial(TutorialQuizConditionFactory.Create(conditionData), onSuccess, onFailed);

    Champion CreateChampion(int id = 1, int speed = 5, params Side[] targetSides)
    {
        var skillDatas = targetSides.Select(side => new SkillData(
            SkillType.StatChanger,
            null,
            null,
            new CardSelectRule(side, TargetRange.Single),
            SkillApplyType.Select,
            SkillValueType.Value,
            StatType.Attack
        ));

        var skill = new Skill(skillDatas);
        var status = new ChampionStatus(new ChampionStatData(10, 10, speed));
        return new Champion(id, skill, status);
    }

    [Test]
    public void 선택이_정답이면_성공_콜백_실행()
    {
        int[] answers = { 1, 2, 3 };
        bool isSuccessCalled = false;
        bool isFailedCalled = false;

        var sut = CreateSut(answers, () => isSuccessCalled = true, () => isFailedCalled = true);

        sut.Select(2);

        Assert.IsTrue(isSuccessCalled);
        Assert.IsFalse(isFailedCalled);
    }

    [Test]
    public void 선택이_오답이면_실패_콜백_실행()
    {
        int[] answers = { 1, 2, 3 };
        bool isSuccessCalled = false;
        bool isFailedCalled = false;

        var sut = CreateSut(answers, () => isSuccessCalled = true, () => isFailedCalled = true);

        sut.Select(999);

        Assert.IsFalse(isSuccessCalled);
        Assert.IsTrue(isFailedCalled);
    }

    [Test]
    public void 속도_조건_이상인_챔피언_선택시_성공_콜백_실행()
    {
        bool isSuccessCalled = false;
        bool isFailedCalled = false;
        var conditionData = new TutorialQuizConditionData(TutorialQuizType.SpeedAtLeast, 6);
        var sut = CreateSut(conditionData, () => isSuccessCalled = true, () => isFailedCalled = true);

        sut.SelectChampion(CreateChampion(speed: 6));

        Assert.IsTrue(isSuccessCalled);
        Assert.IsFalse(isFailedCalled);
    }

    [Test]
    public void 속도_조건_미만인_챔피언_선택시_실패_콜백_실행()
    {
        bool isSuccessCalled = false;
        bool isFailedCalled = false;
        var conditionData = new TutorialQuizConditionData(TutorialQuizType.SpeedAtLeast, 6);
        var sut = CreateSut(conditionData, () => isSuccessCalled = true, () => isFailedCalled = true);

        sut.SelectChampion(CreateChampion(speed: 5));

        Assert.IsFalse(isSuccessCalled);
        Assert.IsTrue(isFailedCalled);
    }

    [Test]
    public void 상대_선택_룰을_가진_챔피언_선택시_성공_콜백_실행()
    {
        bool isSuccessCalled = false;
        bool isFailedCalled = false;
        var conditionData = new TutorialQuizConditionData(TutorialQuizType.TargetOpponent, 0);
        var sut = CreateSut(conditionData, () => isSuccessCalled = true, () => isFailedCalled = true);

        sut.SelectChampion(CreateChampion(targetSides: Side.Opponent));

        Assert.IsTrue(isSuccessCalled);
        Assert.IsFalse(isFailedCalled);
    }

    [Test]
    public void 아군_선택_룰을_가진_챔피언_선택시_성공_콜백_실행()
    {
        bool isSuccessCalled = false;
        bool isFailedCalled = false;
        var conditionData = new TutorialQuizConditionData(TutorialQuizType.TargetSelf, 0);
        var sut = CreateSut(conditionData, () => isSuccessCalled = true, () => isFailedCalled = true);

        sut.SelectChampion(CreateChampion(targetSides: Side.Self));

        Assert.IsTrue(isSuccessCalled);
        Assert.IsFalse(isFailedCalled);
    }
}