public class StageProgressUseCase
{
    readonly IStageStorage _storage;
    readonly MasteryProfile _liveProfile;
    readonly IMasterySaver _masterySaver;
    readonly int EARN_POINT;

    public StageProgressUseCase(IStageStorage storage, MasteryProfile liveProfile, IMasterySaver masterySaver, int earnPoint)
    {
        _storage = storage;
        _liveProfile = liveProfile;
        _masterySaver = masterySaver;
        EARN_POINT = earnPoint;
    }

    public void ClearStage(int stageIndex)
    {
        int current = _storage.LoadUnlockedStage();

        if (stageIndex + 1 > current)
        {
            _storage.SaveUnlockedStage(stageIndex + 1);
            _liveProfile.EarnPoints(EARN_POINT);
            _masterySaver.Save(_liveProfile);
        }
    }
}