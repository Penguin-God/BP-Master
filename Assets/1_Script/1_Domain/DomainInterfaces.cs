using System.Collections.Generic;

public interface ISkillActionFactory
{
    ISkillAction CreateAction(SkillType actionType, SkillAmountData amountData, ChampionStatus caster, Team team);
}

public interface ISkillAction
{
    void Do(ChampionStatus target);
}

public interface IChampionCondition
{
    bool Check(ChampionStatus target);
}

public interface ISkillAmountCalculator
{
    int Calculate(int currentValue);
}

public interface IChampionSelector
{
    int Select(HashSet<int> ids);
}

public interface IChampionEvaluator
{
    int Evaluate(Champion champion);
}

public interface ISkillTargetSelector
{
    IEnumerable<SlotData> SelectTargets(IEnumerable<SlotData> candidates, int count, Skill skill);
}

public interface IMasteryStatProvider
{
    ChampionStatData GetMasteryStat(int championId);
}

public interface IPhaseEvent
{
    void Dispatch(GamePhase phase, Team turn);
}