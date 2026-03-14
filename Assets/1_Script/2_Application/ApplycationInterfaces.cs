public interface IScheduleStorage
{
    void SaveIndex(int index);
    int LoadIndex();
}

public interface ISceneLoader
{
    void LoadBattleScene(MatchData match);
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