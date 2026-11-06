using System.Collections.Generic;

public static class BattleResultLogic
{
    public static int GetWinnerPlayerId(IReadOnlyDictionary<Team, int> playerIds, Team winner)
    {
        var winningTeam = winner == Team.All ? Team.Red : winner;
        return playerIds[winningTeam];
    }
}