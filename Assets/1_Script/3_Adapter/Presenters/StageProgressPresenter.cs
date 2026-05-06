using System.Collections.Generic;
using System.Linq;

public class StageProgressPresenter
{
    readonly IStageStorage _storage;
    int _unlockedIndex;

    public StageProgressPresenter(IStageStorage storage)
    {
        _storage = storage;
        _unlockedIndex = _storage.LoadUnlockedStage();
    }

    public void ClearStage(int stageIndex)
    {
        if (stageIndex >= _unlockedIndex)
        {
            _unlockedIndex = stageIndex + 1;
            _storage.SaveUnlockedStage(_unlockedIndex);
        }
    }

    // UI 버튼 개수에 맞춰 현재 해금 인덱스보다 작거나 같은 요소만 true로 맵핑합니다.
    public IReadOnlyList<bool> GetButtonStates(int totalStages) => Enumerable.Range(0, totalStages).Select(i => i <= _unlockedIndex).ToList();
}