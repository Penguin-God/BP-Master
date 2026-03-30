public interface IScheduleStorage
{
    void SaveIndex(int index);
    int LoadIndex();
}

public interface ISceneLoader
{
    void LoadBattleScene();
}

public interface IAiBattleResolver
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