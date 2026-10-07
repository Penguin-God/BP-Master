
public class PlayerData
{
    public readonly int Id ;
    public readonly string Name;
    public readonly MasteryBoardCollection MasteryBoardCollection;

    public PlayerData(int id, string name, MasteryBoardCollection masteryBoardCollection)
    {
        Id = id;
        Name = name;
        MasteryBoardCollection = masteryBoardCollection;
    }
}
