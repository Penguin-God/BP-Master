using System;
using System.Collections.Generic;

public class CheckSelectTutorial
{
    readonly HashSet<int> _answerSet;
    readonly Func<Champion, bool> _condition;
    readonly Action _onSuccess;
    readonly Action _onFailed;

    public CheckSelectTutorial(IEnumerable<int> answers, Action onSuccess, Action onFailed)
    {
        _answerSet = new HashSet<int>(answers);
        _onSuccess = onSuccess;
        _onFailed = onFailed;
    }

    public CheckSelectTutorial(Func<Champion, bool> condition, Action onSuccess, Action onFailed)
    {
        _condition = condition;
        _onSuccess = onSuccess;
        _onFailed = onFailed;
    }

    public void Select(int selectedId)
    {
        if (_answerSet.Contains(selectedId)) _onSuccess?.Invoke();
        else _onFailed?.Invoke();
    }

    public void SelectChampion(Champion champion)
    {
        if (champion != null && _condition(champion)) _onSuccess?.Invoke();
        else _onFailed?.Invoke();
    }
}