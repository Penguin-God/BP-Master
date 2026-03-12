using System.Collections.Generic;
using System.Linq;

namespace Match
{
    public static class MatchContext
    {
        public static MatchData CurrentMatch { get; private set; } = new MatchData(0, 0);
        public static BanPickStorage Storage { get; private set; }
        public static MatchWinCounter WinCounter { get; private set; }
        public static ParticipantRepository ParticipantRepository { get; private set; }
        static Dictionary<int, PlayerData> _dataByid = new Dictionary<int, PlayerData>();
        static IEnumerable<int> _selectableIds = Enumerable.Empty<int>();

        public static void MatchInit(MatchData match, int targetWin, int[] masteryLevels, IEnumerable<int> allChampionIds)
        {
            CurrentMatch = match;
            WinCounter = new MatchWinCounter(match, targetWin);
            _selectableIds = allChampionIds.ToList();
            Storage = new BanPickStorage(_selectableIds);

            ParticipantRepository = new ParticipantRepository();
            var drawer = new MasteryDrawer(allChampionIds);

            ParticipantRepository.Save(Participant.Player, new PlayerData("Player", new MasteryCollection(drawer.DrawRandoms(masteryLevels))));
            ParticipantRepository.Save(Participant.AI, new PlayerData("AI", new MasteryCollection(drawer.DrawRandoms(masteryLevels))));
        }

        public static void MatchInit(MatchData match, int targetWin, Dictionary<int, PlayerData> dataByid, IEnumerable<int> allChampionIds)
        {
            CurrentMatch = match;
            WinCounter = new MatchWinCounter(match, targetWin);
            _selectableIds = allChampionIds.ToList();
            Storage = new BanPickStorage(_selectableIds);
            _dataByid = dataByid;
        }

        public static PlayerData GetPlayerData(int id) => _dataByid.TryGetValue(id, out var data) ? data : null;

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
            ParticipantRepository = null;
            _dataByid.Clear();
        }
    }
}