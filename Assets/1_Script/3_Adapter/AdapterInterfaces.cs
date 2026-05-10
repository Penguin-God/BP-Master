public interface IMasteryPointView
{
    void UpdatePoints(int points);
    void UpdateChampionDetail(ChampionTextModel champModel, MasteryLevelModel masteryModel);
}

public interface IChampionProvider
{
    ChampionProfile GetProfile(int id);
}