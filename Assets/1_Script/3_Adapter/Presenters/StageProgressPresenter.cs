using System.Collections.Generic;
using System.Linq;

public class StageProgressPresenter
{
    readonly IStageStorage _storage;
    public StageProgressPresenter(IStageStorage storage) => _storage = storage;

    public IReadOnlyList<bool> GetButtonStates(int totalStages)
    {
        int currentUnlocked = _storage.LoadUnlockedStage();
        return Enumerable.Range(0, totalStages).Select(i => i <= currentUnlocked).ToList();
    }
}