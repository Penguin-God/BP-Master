public class ProfileMasteryConverter : IMasteryStatProvider
{
    readonly MasteryBoardCollection _boards;
    readonly MasteryMultiplier _multiplier;

    public ProfileMasteryConverter(MasteryBoardCollection boards, MasteryMultiplier multiplier)
    {
        _boards = boards;
        _multiplier = multiplier;
    }

    public ChampionStatData GetMasteryStat(int championId)
    {
        if (_boards.TryGetBoard(championId, out var board))
        {
            return new ChampionStatData(
                board.AttackLevel * _multiplier.Attack,
                board.DefenseLevel * _multiplier.Defense,
                board.SpeedLevel * _multiplier.Speed
            );
        }

        return default;
    }
}