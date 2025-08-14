using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager
{
    public GamePhase CurrentGamePhase { get; set; }

    public GameFlowManager(IReadOnlyList<GamePhase> phases, IReadOnlyList<Team> banTurns, IReadOnlyList<Team> pickTurns)
    {

    }

    public void Ban(int id)
    {

    }

    public void Pick(int id)
    {

    }

    public void Swap(int index1, int index2)
    {

    }
}
