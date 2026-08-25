using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Match
{
    public static class MatchContext
    {
        static IEnumerable<int> _allChampionIds = Enumerable.Empty<int>();

        public static MatchSeriesState MatchState { get; private set; }
        public static DeckBuildState CurrentDeck;
        public static HashSet<int> FearlessLockedCards { get; private set; } = new HashSet<int>();

        public static void MatchInit(int id1, int id2, int targetWin, IEnumerable<int> allChampionIds)
        {
            MatchState = new MatchSeriesState(new MatchParticipant(id1), new MatchParticipant(id2), targetWin);
            _allChampionIds = allChampionIds.ToList();
            FearlessLockedCards.Clear();
        }

        public static void MatchInit(MatchData matchData, int targetWin, IEnumerable<int> allChampionIds)
        {
            MatchState = new MatchSeriesState(new MatchParticipant(matchData.Id1), new MatchParticipant(matchData.Id2), targetWin);
            _allChampionIds = allChampionIds.ToList();
            FearlessLockedCards.Clear();
        }

        public static bool EndMatch(int winner)
        {
            MatchState = MatchState.AddWin(winner);

            if (MatchState.IsMatchFinished)
            {
                Clear();
                return true;
            }

            return false;
        }

        public static void RecordMatchResult(IEnumerable<int> pickIds)
        {
            FearlessLockedCards.AddRange(pickIds);
        }

        public static BanPickStorage CreateFearlessStorage() => new BanPickStorage(_allChampionIds.Except(FearlessLockedCards));

        public static void Clear()
        {
            FearlessLockedCards.Clear();
            MatchState = null;
            _allChampionIds = Enumerable.Empty<int>();
            CurrentDeck = null;
        }
    }
}