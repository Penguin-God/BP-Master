using System;
using System.Collections.Generic;

public interface ISkillAction
{
    void Do(ChampionStatus target);
}

public interface IChampionCondition
{
    bool Check(ChampionStatus target, ChampionStatus caster);
}

public interface ISkillAmountCalculator
{
    int Calculate(int currentValue);
}

public interface ISkillRegister
{
    void Register(Action<ChampionStatus> execute);
}

public interface IChampionSelector
{
    int Select(HashSet<int> ids);
}