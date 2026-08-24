using System.Collections.Generic;
using System.Linq;

namespace Match
{
    public static class MatchContext
    {
        public static DeckBuildState CurrentDeck;
        public static MatchData CurrentMatch { get; private set; } = new MatchData(0, 0);
        public static BanPickStorage Storage { get; private set; }
        public static MatchWinCounter WinCounter { get; private set; }
        static IEnumerable<int> _selectableIds = Enumerable.Empty<int>();

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

        public static void Draw() => Storage = new BanPickStorage(_selectableIds);

        public static void Clear()
        {
            CurrentMatch = new MatchData(0, 0);
            _selectableIds = Enumerable.Empty<int>();
            Storage = null;
            WinCounter = null;
        }
    }
}