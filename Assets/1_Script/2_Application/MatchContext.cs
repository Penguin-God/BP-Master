using Sirenix.Utilities;
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
        static IEnumerable<int> _allChampionIds = Enumerable.Empty<int>();
        public static HashSet<int> FearlessLockedCards { get; private set; } = new HashSet<int>();


        public static void MatchInit(MatchData matchData, int targetWin, IEnumerable<int> allChampionIds)
        {
            CurrentMatch = matchData;

            WinCounter = new MatchWinCounter(CurrentMatch, targetWin);
            _allChampionIds = allChampionIds.ToList();
            Storage = new BanPickStorage(_allChampionIds);
        }

        public static bool EndMatch(int winner)
        {
            WinCounter.AddWin(winner);
            if (WinCounter.IsMatchFinished)
            {
                Clear();
                return true;
            }

            Storage = null;
            return false;
        }

        public static void RecordMatchResult(IEnumerable<int> pickIds)
        {
            FearlessLockedCards.AddRange(pickIds);
        }

        public static BanPickStorage CreateFearlessStorage() => new BanPickStorage(_allChampionIds.Except(FearlessLockedCards));

        public static void Draw() => Storage = null;

        public static void Clear()
        {
            FearlessLockedCards.Clear();
            CurrentMatch = new MatchData(0, 0);
            _allChampionIds = Enumerable.Empty<int>();
            Storage = null;
            WinCounter = null;
        }
    }
}