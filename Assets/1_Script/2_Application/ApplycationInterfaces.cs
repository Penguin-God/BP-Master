public interface IPhaseEntry
{
    void EnterBan();
    void EnterPick();
}

public interface IPlayerDataLoader
{
    PlayerData LoadPlayer(int id);
}

public interface IStageStorage
{
    int LoadUnlockedStage();
    void SaveUnlockedStage(int stageIndex);
}

public interface IMasterySaver
{
    void Save(MasteryProfile inventory);
}