public class StageProgressUseCase
{
    readonly IStageStorage _storage;
    public StageProgressUseCase(IStageStorage storage) => _storage = storage;

    public void ClearStage(int stageIndex)
    {
        int current = _storage.LoadUnlockedStage();
        if (stageIndex + 1 > current)
        {
            _storage.SaveUnlockedStage(stageIndex + 1);
        }
    }
}