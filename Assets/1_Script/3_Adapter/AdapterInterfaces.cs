public interface IMasteryPointView
{
    void UpdatePoints(int points);
    void UpdateChampionDetail(ChampionTextModel champModel, MasteryLevelModel masteryModel);
}

public interface IMasterySaver
{
    void Save(MasteryProfile inventory);
}

public interface IChampionProvider
{
    ChampionProfile GetProfile(int id);
}

public interface IStageStorage
{
    int LoadUnlockedStage();
    void SaveUnlockedStage(int stageIndex);
}