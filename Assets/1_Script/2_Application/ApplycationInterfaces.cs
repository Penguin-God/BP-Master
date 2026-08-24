
public interface IBattleResolver
{
    void Resolve(MatchData match);
}

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

public interface ITutorialStorage
{
    bool HasSeen(TutorialType type);
    void MarkAsSeen(TutorialType type);
}

public interface ITutorialViewer
{
    void Show(TutorialType type);
}