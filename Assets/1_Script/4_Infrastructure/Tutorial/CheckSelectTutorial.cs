using System;
using System.Collections.Generic;

public class CheckSelectTutorial
{
    readonly HashSet<int> _answerSet;
    readonly Action _onSuccess;
    readonly Action _onFailed;

    public CheckSelectTutorial(IEnumerable<int> answers, Action onSuccess, Action onFailed)
    {
        _answerSet = new HashSet<int>(answers ?? Array.Empty<int>());
        _onSuccess = onSuccess;
        _onFailed = onFailed;
    }

    public void Select(int selectedId)
    {
        if (_answerSet.Contains(selectedId)) _onSuccess?.Invoke();
        else _onFailed?.Invoke();
    }
}