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
    LeagueRecordCollection LoadAll();
    void SaveAll(LeagueRecordCollection collection);
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