using System.Collections.Generic;
using System.Linq;
public class PlayerMatchData
{
    public readonly PlayerData Player1;
    public readonly PlayerData Player2;

    public PlayerMatchData(PlayerData player1, PlayerData player2)
    {
        Player1 = player1;
        Player2 = player2;
    }

    public PlayerData GetPlayer(int id)
    {
        if (Player1.Id == id) return Player1;
        if (Player2.Id == id) return Player2;

        return null;
    }

    public MatchData ToMatchData() => new MatchData(Player1.Id, Player2.Id);
}

namespace Match
{
    public static class MatchContext
    {
        public static MatchData CurrentMatch { get; private set; } = new MatchData(0, 0);
        public static BanPickStorage Storage { get; private set; }
        public static MatchWinCounter WinCounter { get; private set; }

        static IEnumerable<int> _selectableIds = Enumerable.Empty<int>();

        public static void MatchInit(PlayerMatchData playerMatchData, int targetWin, IEnumerable<int> allChampionIds)
        {
            CurrentMatch = playerMatchData.ToMatchData();

            WinCounter = new MatchWinCounter(CurrentMatch, targetWin);
            _selectableIds = allChampionIds.ToList();
            Storage = new BanPickStorage(_selectableIds);
        }

        public static void MatchInit(MatchData matchData, int targetWin, IEnumerable<int> allChampionIds)
        {
            CurrentMatch = matchData;

            WinCounter = new MatchWinCounter(CurrentMatch, targetWin);
            _selectableIds = allChampionIds.ToList();
            Storage = new BanPickStorage(_selectableIds);
        }

        public static bool EndMatch(int winner)
        {
            WinCounter.AddWin(winner);
            if (WinCounter.IsMatchFinished)
            {
                Clear();
                return true;
            }

            _selectableIds = _selectableIds.Except(Storage.PickIds.GetAll()).ToList();
            Storage = new BanPickStorage(_selectableIds);
            return false;
        }

        public static void Clear()
        {
            CurrentMatch = new MatchData(0, 0);
            _selectableIds = Enumerable.Empty<int>();
            Storage = null;
            WinCounter = null;
        }
    }
}