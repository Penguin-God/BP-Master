using System;
using System.Collections.Generic;

public class CheckSelectTutorial
{
    readonly Func<Champion, bool> _condition;
    readonly Action _onSuccess;
    readonly Action _onFailed;

    public CheckSelectTutorial(Func<Champion, bool> condition, Action onSuccess, Action onFailed)
    {
        _condition = condition;
        _onSuccess = onSuccess;
        _onFailed = onFailed;
    }

    public void SelectChampion(Champion champion)
    {
        if (champion != null && _condition(champion)) _onSuccess?.Invoke();
        else _onFailed?.Invoke();
    }
}