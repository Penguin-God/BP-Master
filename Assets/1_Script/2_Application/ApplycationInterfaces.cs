using System.Collections.Generic;

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

public interface ILeagueRecordStorage
{
    Dictionary<int, LeagueRecord> LoadAll();
    void SaveAll(Dictionary<int, LeagueRecord> records);
}