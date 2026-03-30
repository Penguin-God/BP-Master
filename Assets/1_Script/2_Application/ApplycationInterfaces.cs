public interface IScheduleStorage
{
    void SaveIndex(int index);
    int LoadIndex();
}

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