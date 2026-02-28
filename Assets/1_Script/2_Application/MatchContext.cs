using System.Collections.Generic;
using System.Linq;

namespace Match
{
    public static class MatchContext
    {
        public static MatchData CurrentMatch { get; private set; } = new MatchData(0, 0);
        public static BanPickStorage Storage { get; private set; }
        public static MatchRecord Record { get; private set; }
        public static ParticipantRepository ParticipantRepository { get; private set; }

        static IEnumerable<int> _selectableIds = Enumerable.Empty<int>();

        public static void MatchInit(MatchData match, int targetWin, int[] masteryLevels, IEnumerable<int> allChampionIds)
        {
            CurrentMatch = match;
            Record = new MatchRecord(targetWin);
            _selectableIds = allChampionIds.ToList();
            Storage = new BanPickStorage(_selectableIds);

            ParticipantRepository = new ParticipantRepository();
            var drawer = new MasteryDrawer(allChampionIds);

            ParticipantRepository.Save(Participant.Player, new ParticipantData("Player", new MasteryCollection(drawer.DrawRandoms(masteryLevels))));
            ParticipantRepository.Save(Participant.AI, new ParticipantData("AI", new MasteryCollection(drawer.DrawRandoms(masteryLevels))));
        }

        public static void EndMatch(Participant participant)
        {
            _selectableIds = _selectableIds.Except(Storage.PickIds.GetAll()).ToList();
            Storage = new BanPickStorage(_selectableIds);
            Record.AddWin(participant);
        }

        public static void Clear()
        {
            CurrentMatch = new MatchData(0, 0);
            _selectableIds = Enumerable.Empty<int>();
            Storage = null;
            Record = null;
            ParticipantRepository = null;
        }
    }
}